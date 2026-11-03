using SistemaGestion.Web.Models;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Services;

/// <summary>Ventas de demostración en memoria; las reglas de negocio permanecen en Venta.</summary>
public sealed class VentasEnMemoria(CatalogoEnMemoria catalogo, ClientesEnMemoria clientes)
{
    private readonly object _sync = new();
    private readonly List<Venta> _ventas = [];
    private int _siguienteId = 1;
    private readonly IReadOnlyList<MedioPago> _medios =
    [
        new MedioPago(1, TipoMedioPago.Efectivo, "Efectivo"),
        new MedioPago(2, TipoMedioPago.TarjetaDebito, "Tarjeta de débito"),
        new MedioPago(3, TipoMedioPago.TarjetaCredito, "Tarjeta de crédito"),
        new MedioPago(4, TipoMedioPago.Transferencia, "Transferencia")
    ];

    public IReadOnlyList<(int Id, string Nombre)> MediosPago() =>
        _medios.Select(m => (m.obtenerId(), m.obtenerDescripcion())).ToArray();

    public VentasIndexViewModel Listar()
    {
        lock (_sync)
        {
            var filas = _ventas.OrderByDescending(v => v.ObtenerFecha()).ThenByDescending(v => v.ObtenerId())
                .Select(Fila).ToArray();
            return new VentasIndexViewModel
            {
                Ventas = filas, TotalVentas = filas.Count(v => v.Estado == EstadoVenta.Completada),
                Ingresos = filas.Where(v => v.Estado == EstadoVenta.Completada).Sum(v => v.Total)
            };
        }
    }

    public (int Cantidad, decimal Importe) Hoy()
    {
        lock (_sync)
        {
            var hoy = _ventas.Where(v => v.ObtenerEstado() == EstadoVenta.Completada &&
                v.ObtenerFecha().Date == DateTime.Today).ToArray();
            return (hoy.Length, hoy.Sum(v => v.CalcularTotal()));
        }
    }

    public VentaDetalleViewModel? Obtener(int id)
    {
        lock (_sync)
        {
            var venta = _ventas.FirstOrDefault(v => v.ObtenerId() == id);
            return venta is null ? null : new VentaDetalleViewModel
            {
                Venta = Fila(venta),
                Detalles = venta.ObtenerDetalles().Select(d => new DetalleVentaFilaViewModel(
                    d.getProducto().obtenerNombre(), d.getCantidad(), d.getPrecioUnitario(),
                    d.calcularSubtotal())).ToArray()
            };
        }
    }

    public int CrearYConfirmar(VentaFormViewModel datos)
    {
        lock (_sync)
        {
            var cliente = clientes.ObtenerInstancia(datos.ClienteId)
                ?? throw new ArgumentException("Seleccioná un cliente válido.");
            var medio = _medios.FirstOrDefault(m => m.obtenerId() == datos.MedioPagoId)
                ?? throw new ArgumentException("Seleccioná un medio de pago válido.");
            var lineas = (datos.Lineas ?? []).Where(x => x.ProductoId != 0 || x.Cantidad != 0).ToArray();
            if (lineas.Length == 0) throw new ArgumentException("Agregá al menos un producto.");
            if (lineas.Any(x => x.ProductoId <= 0 || x.Cantidad <= 0))
                throw new ArgumentException("Cada producto debe tener una cantidad mayor a cero.");
            if (lineas.GroupBy(x => x.ProductoId).Any(g => g.Count() > 1))
                throw new ArgumentException("No repitas productos; reuní las unidades en una sola línea.");

            var venta = new Venta(_siguienteId, DateTime.Now, cliente, medio);
            foreach (var linea in lineas)
            {
                var producto = catalogo.ObtenerProductoParaVenta(linea.ProductoId)
                    ?? throw new ArgumentException("Seleccioná un producto válido.");
                venta.AgregarDetalle(new DetalleVenta(producto, linea.Cantidad, producto.obtenerPrecio()));
            }
            // Este método es atómico respecto de otros cambios en el catálogo.
            catalogo.ConfirmarVentaConMovimientos(venta);
            clientes.RegistrarCompra(cliente, venta);
            _ventas.Add(venta);
            _siguienteId++;
            return venta.ObtenerId();
        }
    }

    private static VentaFilaViewModel Fila(Venta v) => new(
        v.ObtenerId(), v.ObtenerFecha(), v.ObtenerCliente().getNombre(),
        v.ObtenerMedioPago().obtenerDescripcion(), v.ObtenerEstado(), v.CalcularTotal(),
        v.ObtenerDetalles().Count);
}
