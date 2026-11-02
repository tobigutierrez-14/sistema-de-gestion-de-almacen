using System.ComponentModel.DataAnnotations;

namespace SistemaGestion.Web.ViewModels;

public sealed class CategoriaFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre de la categoría.")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción de la categoría es obligatoria.")]
    [StringLength(400, ErrorMessage = "La descripción no puede superar los 400 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;
}
