namespace SistemaGestion.Web.ViewModels;

public sealed class DashboardViewModel
{
    public int TotalProductos { get; init; }
    public int ProductosBajoStock { get; init; }
    // Ventas y movimientos aún no están implementados; no se muestran cifras inventadas.
    public IReadOnlyList<ActividadReciente> Actividades { get; init; } = [];
    public IReadOnlyList<AlertaStock> Alertas { get; init; } = [];
}
public sealed record ActividadReciente(string Titulo, string Detalle, string Hora, string Tipo);
public sealed record AlertaStock(string Nombre, string Presentacion, int Unidades);
