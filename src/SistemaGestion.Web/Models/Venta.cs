namespace SistemaGestion.Web.Models
{
    public class Venta
    {
        // Atributos
        private int id;
        private DateTime fecha;
        private Cliente cliente;
        private List<DetalleVenta> detalles;
        private MedioPago medioPago;
        private EstadoVenta estado;

        // Constructor
        public Venta(int id,DateTime fecha, Cliente cliente, MedioPago medioPago)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id), "El ID debe ser mayor que cero.");

            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente));

            if (medioPago == null)
                throw new ArgumentNullException(nameof(medioPago));

            this.id = id;
            this.fecha = fecha;
            this.cliente = cliente;
            this.medioPago = medioPago;
            this.detalles = new List<DetalleVenta>();
            this.estado = EstadoVenta.Pendiente;
        }

        // Agregar un detalle a la venta
        public void AgregarDetalle(DetalleVenta detalle)
        {
            if (estado != EstadoVenta.Pendiente)
                throw new InvalidOperationException(
                    "Solo se pueden agregar detalles a una venta pendiente.");

            if (detalle == null)
                throw new ArgumentNullException(nameof(detalle));

            detalles.Add(detalle);
        }

        // Calcular el total de la venta
        public decimal CalcularTotal()
        {
            return detalles.Sum(
                detalle => detalle.calcularSubtotal());
        }

        // Confirmar la venta y descontar el stock
        public void ConfirmarVenta()
        {
            if (estado != EstadoVenta.Pendiente)
                throw new InvalidOperationException(
                    "La venta no está pendiente.");

            if (detalles.Count == 0)
                throw new InvalidOperationException(
                    "No se puede confirmar una venta sin detalles.");

            // Agrupar los detalles del mismo producto para validar la cantidad total que se desea vender.
            var productosAgrupados = detalles.GroupBy(detalle => detalle.getProducto()).Select(grupo => new
                {
                    Producto = grupo.Key,
                    CantidadTotal = grupo.Sum(
                        detalle => detalle.getCantidad())
                })
                .ToList();

            // Primero validar todo el stock.
            foreach (var item in productosAgrupados)
            {
                if (!item.Producto.HayStockDisponible(item.CantidadTotal))
                {
                    throw new InvalidOperationException("Stock insuficiente para el producto: " + item.Producto.obtenerNombre());
                }
            }

            // Solo después de validar todos los productos, descontar las cantidades correspondientes.
            foreach (var item in productosAgrupados)
            {
                item.Producto.DisminuirStock(item.CantidadTotal);
            }

            estado = EstadoVenta.Completada;
        }

        // Cancelar una venta pendiente
        public void CancelarVenta()
        {
            if (estado != EstadoVenta.Pendiente)
                throw new InvalidOperationException("Solo se puede cancelar una venta pendiente.");

            estado = EstadoVenta.Cancelada;
        }

        // Getters
        public int ObtenerId()
        {
            return id;
        }

        public DateTime ObtenerFecha()
        {
            return fecha;
        }

        public Cliente ObtenerCliente()
        {
            return cliente;
        }

        public MedioPago ObtenerMedioPago()
        {
            return medioPago;
        }

        public EstadoVenta ObtenerEstado()
        {
            return estado;
        }

        public IReadOnlyList<DetalleVenta> ObtenerDetalles()
        {
            return detalles.AsReadOnly();
        }
    }
}