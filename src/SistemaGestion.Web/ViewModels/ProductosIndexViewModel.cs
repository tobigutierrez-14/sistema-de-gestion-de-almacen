namespace SistemaGestion.Web.ViewModels;

public sealed record ProductoFilaViewModel(int Id, string Nombre, string Descripcion, decimal Precio, int Stock, int StockMinimo, string Categoria, bool StockBajo);
public sealed record CategoriaOpcionViewModel(int Id, string Nombre);
public sealed class ProductosIndexViewModel
{
    public string Busqueda { get; init; } = "";
    public int? CategoriaId { get; init; }
    public string Estado { get; init; } = "todos";
    public IReadOnlyList<CategoriaOpcionViewModel> Categorias { get; init; } = [];
    public IReadOnlyList<ProductoFilaViewModel> Productos { get; init; } = [];
    public int Total { get; init; }
    public int BajoStock { get; init; }
}
