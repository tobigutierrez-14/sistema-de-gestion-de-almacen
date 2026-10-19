using System.ComponentModel.DataAnnotations;

namespace SistemaGestion.Web.ViewModels;

public sealed class ProductoFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre del producto.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(400, ErrorMessage = "La descripción no puede superar los 400 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "999999999", ErrorMessage = "El precio debe ser mayor o igual a cero.")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo.")]
    public int StockMinimo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná una categoría.")]
    public int CategoriaId { get; set; }
}
