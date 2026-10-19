using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestion.Web.Services;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Controllers;

public sealed class ProductosController(CatalogoEnMemoria catalogo) : Controller
{
    [HttpGet]
    public IActionResult Index(string? busqueda, int? categoriaId, string estado = "todos")
    {
        var todos = catalogo.Productos();
        var filtrados = todos.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(busqueda))
            filtrados = filtrados.Where(x => x.Nombre.Contains(busqueda.Trim(), StringComparison.OrdinalIgnoreCase) ||
                x.Descripcion.Contains(busqueda.Trim(), StringComparison.OrdinalIgnoreCase));
        if (categoriaId.HasValue)
        {
            var categoria = catalogo.Categorias().FirstOrDefault(c => c.Id == categoriaId.Value);
            filtrados = categoria is null ? [] : filtrados.Where(p => p.Categoria == categoria.Nombre);
        }
        if (estado == "bajo") filtrados = filtrados.Where(p => p.StockBajo);
        else if (estado == "normal") filtrados = filtrados.Where(p => !p.StockBajo);
        else estado = "todos";
        return View(new ProductosIndexViewModel
        {
            Busqueda = busqueda ?? "", CategoriaId = categoriaId, Estado = estado,
            Categorias = catalogo.Categorias(), Productos = filtrados.OrderBy(p => p.Nombre).ToArray(),
            Total = todos.Count, BajoStock = todos.Count(p => p.StockBajo)
        });
    }

    [HttpGet]
    public IActionResult Crear()
    {
        PrepararFormulario("Nuevo producto");
        return View("Formulario", new ProductoFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(ProductoFormViewModel datos)
    {
        Validar(datos);
        if (!ModelState.IsValid) { PrepararFormulario("Nuevo producto"); return View("Formulario", datos); }
        catalogo.Agregar(datos);
        TempData["Exito"] = "Producto agregado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var datos = catalogo.Obtener(id);
        if (datos is null) return NotFound();
        PrepararFormulario("Editar producto");
        return View("Formulario", datos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, ProductoFormViewModel datos)
    {
        if (catalogo.Obtener(id) is null) return NotFound();
        Validar(datos);
        if (!ModelState.IsValid) { PrepararFormulario("Editar producto"); return View("Formulario", datos); }
        if (!catalogo.Actualizar(id, datos)) return NotFound();
        TempData["Exito"] = "Producto actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private void Validar(ProductoFormViewModel datos)
    {
        if (!catalogo.CategoriaExiste(datos.CategoriaId))
            ModelState.AddModelError(nameof(datos.CategoriaId), "Seleccioná una categoría válida.");
        if (string.IsNullOrWhiteSpace(datos.Nombre))
            ModelState.AddModelError(nameof(datos.Nombre), "Ingresá el nombre del producto.");
    }

    private void PrepararFormulario(string titulo)
    {
        ViewData["Title"] = titulo;
        ViewData["Categorias"] = catalogo.Categorias().Select(c => new SelectListItem(c.Nombre, c.Id.ToString())).ToList();
    }
}
