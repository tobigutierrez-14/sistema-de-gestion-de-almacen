using Microsoft.AspNetCore.Mvc;
using SistemaGestion.Web.Services;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Controllers;

public sealed class CategoriasController(CatalogoEnMemoria catalogo) : Controller
{
    [HttpGet]
    public IActionResult Index(string? busqueda)
    {
        var categorias = catalogo.ListarCategorias();
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var filtro = busqueda.Trim();
            categorias = categorias.Where(c =>
                c.Nombre.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                c.Descripcion.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        ViewData["Busqueda"] = busqueda ?? "";
        return View(categorias);
    }

    [HttpGet]
    public IActionResult Crear() => View("Formulario", new CategoriaFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(CategoriaFormViewModel datos)
    {
        ValidarNombre(datos);
        if (!ModelState.IsValid) return View("Formulario", datos);
        try { catalogo.AgregarCategoria(datos); }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(nameof(datos.Nombre), ex.Message);
            return View("Formulario", datos);
        }
        TempData["Exito"] = "Categoría registrada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var datos = catalogo.ObtenerCategoria(id);
        return datos is null ? NotFound() : View("Formulario", datos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, CategoriaFormViewModel datos)
    {
        if (catalogo.ObtenerCategoria(id) is null) return NotFound();
        ValidarNombre(datos, id);
        if (!ModelState.IsValid) { datos.Id = id; return View("Formulario", datos); }
        try
        {
            if (!catalogo.ActualizarCategoria(id, datos)) return NotFound();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(nameof(datos.Nombre), ex.Message);
            datos.Id = id;
            return View("Formulario", datos);
        }
        TempData["Exito"] = "Categoría actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private void ValidarNombre(CategoriaFormViewModel datos, int? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(datos.Nombre)) return;
        if (catalogo.NombreCategoriaOcupado(datos.Nombre, idExcluir))
            ModelState.AddModelError(nameof(datos.Nombre), "Ya existe una categoría con ese nombre.");
    }
}
