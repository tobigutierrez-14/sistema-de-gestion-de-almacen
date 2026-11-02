using System.ComponentModel.DataAnnotations;

namespace SistemaGestion.Web.ViewModels;

public sealed class ClienteFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre del cliente.")]
    [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el DNI del cliente.")]
    [RegularExpression(@"^[0-9]{1,12}$", ErrorMessage = "El DNI debe contener solamente números (hasta 12 dígitos).")]
    public string Dni { get; set; } = string.Empty;

    [StringLength(40, ErrorMessage = "El teléfono no puede superar los 40 caracteres.")]
    public string? Telefono { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Ingresá un correo electrónico válido.")]
    [StringLength(160, ErrorMessage = "El correo no puede superar los 160 caracteres.")]
    public string? Email { get; set; } = string.Empty;
}

public sealed record ClienteFilaViewModel(int Id, string Nombre, string Dni, string Telefono, string Email, int Compras, decimal TotalComprado);
