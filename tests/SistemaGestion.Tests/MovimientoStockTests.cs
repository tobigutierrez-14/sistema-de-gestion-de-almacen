using System;
using Xunit;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Tests;

public class MovimientoStockTests
{
    private Producto CrearProductoAuxiliar(int stockInicial = 30)
    {
        var categoria = new Categoria(1, "Almacén", "Comestibles");
        return new Producto(10, "Fideos 500g", "Fideos secos", 800m, stockInicial, 5, categoria);
    }

    [Fact]
    public void IngresoStock_Aplicar_IncrementaStockDelProducto()
    {
        // Arrange
        var producto = CrearProductoAuxiliar(stockInicial: 20);
        MovimientoStock movimiento = new IngresoStock(producto, 15, "Recepción mercadería", "2026-10-12");

        // Act (comportamiento polimórfico)
        movimiento.Aplicar();

        // Assert
        Assert.Equal("INGRESO", movimiento.getTipo());
        Assert.Equal(35, producto.obtenerStock());
        Assert.Equal(15, movimiento.getCantidad());
        Assert.Equal("Recepción mercadería", movimiento.getMotivo());
        Assert.Equal("2026-10-12", movimiento.getFecha());
    }

    [Fact]
    public void EgresoStock_Aplicar_DisminuyeStockDelProducto()
    {
        // Arrange
        var producto = CrearProductoAuxiliar(stockInicial: 20);
        MovimientoStock movimiento = new EgresoStock(producto, 5, "Venta de mostrador");

        // Act (comportamiento polimórfico)
        movimiento.Aplicar();

        // Assert
        Assert.Equal("EGRESO", movimiento.getTipo());
        Assert.Equal(15, producto.obtenerStock());
    }

    [Fact]
    public void EgresoStock_Aplicar_ConStockInsuficiente_LanzaInvalidOperationException()
    {
        // Arrange (Caso Borde/Error de negocio delegado en Producto)
        var producto = CrearProductoAuxiliar(stockInicial: 3);
        MovimientoStock movimiento = new EgresoStock(producto, 10, "Venta mayorista");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => movimiento.Aplicar());
    }

    [Fact]
    public void CrearMovimiento_ConDatosInvalidos_LanzaExcepciones()
    {
        // Arrange
        var producto = CrearProductoAuxiliar();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new IngresoStock(null!, 10, "Ajuste"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new IngresoStock(producto, 0, "Ajuste"));
        Assert.Throws<ArgumentOutOfRangeException>(() => new EgresoStock(producto, -5, "Ajuste"));
        Assert.Throws<ArgumentException>(() => new IngresoStock(producto, 10, "   "));
    }
}