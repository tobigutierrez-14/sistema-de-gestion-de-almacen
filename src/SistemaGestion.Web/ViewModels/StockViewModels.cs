using System.ComponentModel.DataAnnotations;

namespace SistemaGestion.Web.ViewModels;

public sealed record MovimientoStockFilaViewModel(int Id, string Producto, string Tipo, int Cantidad, string Motivo, string Fecha);

public sealed class StockIndexViewModel
{
    public string Busqueda { get; init; } = "";
    public int? CategoriaId { get; init; }
    public string Estado { get; init; } = "todos";
    public string Tipo { get; init; } = "todos";
    public IReadOnlyList<CategoriaOpcionViewModel> Categorias { get; init; } = [];
    public IReadOnlyList<ProductoFilaViewModel> Productos { get; init; } = [];
    public IReadOnlyList<MovimientoStockFilaViewModel> Movimientos { get; init; } = [];
    public int TotalProductos { get; init; }
    public long TotalUnidades { get; init; }
    public int ProductosBajoStock { get; init; }
}

public sealed class StockMovimientoFormViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná un producto.")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "Seleccioná el tipo de movimiento.")]
    [RegularExpression("^(INGRESO|EGRESO)$", ErrorMessage = "El tipo debe ser ingreso o egreso.")]
    public string Tipo { get; set; } = "INGRESO";

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; } = 1;

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    [StringLength(250, ErrorMessage = "El motivo no puede superar los 250 caracteres.")]
    public string Motivo { get; set; } = "";
}
