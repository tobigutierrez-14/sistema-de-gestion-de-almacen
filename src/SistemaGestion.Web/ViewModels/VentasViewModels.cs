using System.ComponentModel.DataAnnotations;
using SistemaGestion.Web.Models;

namespace SistemaGestion.Web.ViewModels;

public sealed class VentaLineaFormViewModel
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}

public sealed class VentaFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un cliente.")]
    public int ClienteId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un medio de pago.")]
    public int MedioPagoId { get; set; }
    public List<VentaLineaFormViewModel> Lineas { get; set; } =
        Enumerable.Range(0, 5).Select(_ => new VentaLineaFormViewModel()).ToList();
}

public sealed record VentaFilaViewModel(int Id, DateTime Fecha, string Cliente,
    string MedioPago, EstadoVenta Estado, decimal Total, int CantidadProductos);

public sealed record DetalleVentaFilaViewModel(string Producto, int Cantidad, decimal PrecioUnitario, decimal Subtotal);

public sealed class VentaDetalleViewModel
{
    public required VentaFilaViewModel Venta { get; init; }
    public IReadOnlyList<DetalleVentaFilaViewModel> Detalles { get; init; } = [];
}

public sealed class VentasIndexViewModel
{
    public IReadOnlyList<VentaFilaViewModel> Ventas { get; init; } = [];
    public int TotalVentas { get; init; }
    public decimal Ingresos { get; init; }
}
