using Microsoft.AspNetCore.Mvc;

namespace SistemaGestion.Web.Controllers;

public sealed class SeccionesController : Controller
{
    private static readonly IReadOnlyDictionary<string, string> Secciones =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["categorias"] = "Categorías",
            ["ventas"] = "Ventas",
            ["stock"] = "Movimientos de stock",
            ["clientes"] = "Clientes",
            ["proveedores"] = "Proveedores",
        };

    [HttpGet("secciones/{seccion}")]
    public IActionResult Index(string seccion)
    {
        if (!Secciones.TryGetValue(seccion, out var titulo)) return NotFound();
        ViewData["Seccion"] = seccion.ToLowerInvariant();
        ViewData["Title"] = titulo;
        return View(model: titulo);
    }
}
