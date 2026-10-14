using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests
{
    public class VentaTests
    {
        // Crear los objetos necesarios para las pruebas.
        private Cliente CrearCliente()
        {
            return new Cliente("Juan Perez", "30111222", "1122334455", "juan@email.com");
        }

        private Categoria CrearCategoria()
        {
            return new Categoria(1, "Alimentos", "Productos alimenticios");
        }

        private Producto CrearProducto(int stock = 10)
        {
            return new Producto(1, "Arroz", "Arroz integral", 1500m, stock, 3, CrearCategoria());
        }

        private MedioPago CrearMedioPago()
        {
            return new MedioPago(1, TipoMedioPago.Efectivo, "Pago en efectivo");
        }

        private Venta CrearVenta()
        {
            return new Venta(1, DateTime.Now, CrearCliente(), CrearMedioPago());
        }

        [Fact]
        public void CalcularTotal_ConUnDetalle_DevuelveSubtotal()
        {
            // Arrange
            Venta venta = CrearVenta();
            Producto producto = CrearProducto();

            DetalleVenta detalle = new DetalleVenta(producto, 2, 1500m);

            // Act
            venta.AgregarDetalle(detalle);

            // Assert
            Assert.Equal(3000m, venta.CalcularTotal());
        }

        [Fact]
        public void ConfirmarVenta_ConStockSuficiente_DescuentaStock()
        {
            // Arrange
            Venta venta = CrearVenta();
            Producto producto = CrearProducto(stock: 10);

            venta.AgregarDetalle(
                new DetalleVenta(producto, 3, 1500m));

            // Act
            venta.ConfirmarVenta();

            // Assert
            Assert.Equal(7, producto.obtenerStock());
            Assert.Equal(EstadoVenta.Completada, venta.ObtenerEstado());
        }

        [Fact]
        public void ConfirmarVenta_SinDetalles_LanzaExcepcion()
        {
            // Arrange
            Venta venta = CrearVenta();

            // Act y Assert
            Assert.Throws<InvalidOperationException>(() => venta.ConfirmarVenta());
            Assert.Equal(EstadoVenta.Pendiente, venta.ObtenerEstado());
        }

        [Fact]
        public void ConfirmarVenta_SinStockSuficiente_NoConfirmaNiDescuenta()
        {
            // Arrange
            Venta venta = CrearVenta();
            Producto producto = CrearProducto(stock: 5);

            venta.AgregarDetalle(
                new DetalleVenta(producto, 6, 1500m));

            // Act y Assert
            Assert.Throws<InvalidOperationException>(
                () => venta.ConfirmarVenta());

            Assert.Equal(5, producto.obtenerStock());
            Assert.Equal(EstadoVenta.Pendiente, venta.ObtenerEstado());
        }

        [Fact]
        public void AgregarDetalle_ConVentaConfirmada_LanzaExcepcion()
        {
            // Arrange
            Venta venta = CrearVenta();
            Producto producto = CrearProducto();

            venta.AgregarDetalle(
                new DetalleVenta(producto, 2, 1500m));

            venta.ConfirmarVenta();

            // Act y Assert
            Assert.Throws<InvalidOperationException>(() => venta.AgregarDetalle(new DetalleVenta(producto, 1, 1500m)));
            Assert.Single(venta.ObtenerDetalles());

        }
    }
}
//Caso normal: CalcularTotal_ConUnDetalle_DevuelveSubtotal() --> crear una venta, agregar un detalle y verificar el total.
//Caso normal 2: ConfirmarVenta_ConStockSuficiente_DescuentaStock() --> confirmar una venta y comprobar que se descuente el stock.
//Caso borde: ConfirmarVenta_SinDetalles_LanzaExcepcion() --> intentar confirmar una venta sin detalles.
//Caso error: ConfirmarVenta_SinStockSuficiente_NoConfirmaNiDescuenta() --> intentar vender más unidades de las disponibles y comprobar que la venta no se confirme ni se descuente stock.
//Caso estado: AgregarDetalle_ConVentaConfirmada_LanzaExcepcion() --> impedir que se agreguen detalles después de confirmar una venta.