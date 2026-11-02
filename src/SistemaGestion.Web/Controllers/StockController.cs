using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestion.Web.Services;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Controllers;

public sealed class StockController(CatalogoEnMemoria catalogo) : Controller
{
    [HttpGet]
    public IActionResult Index(string? busqueda, int? categoriaId, string? estado, string? tipo)
        => View(catalogo.ResumenStock(busqueda, categoriaId, estado, tipo));

    [HttpGet]
    public IActionResult Registrar(int? productoId)
    {
        PrepararProductos();
        return View(new StockMovimientoFormViewModel { ProductoId = productoId ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Registrar(StockMovimientoFormViewModel datos)
    {
        if (!ModelState.IsValid)
        {
            PrepararProductos();
            return View(datos);
        }
        try { catalogo.RegistrarMovimiento(datos); }
        catch (ArgumentException ex) { ModelState.AddModelError(string.Empty, ex.Message); }
        catch (InvalidOperationException ex) { ModelState.AddModelError(string.Empty, ex.Message); }
        if (!ModelState.IsValid)
        {
            PrepararProductos();
            return View(datos);
        }
        TempData["Exito"] = "Movimiento de stock registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private void PrepararProductos()
    {
        ViewData["Productos"] = catalogo.Productos().OrderBy(p => p.Nombre)
            .Select(p => new SelectListItem($"{p.Nombre} (stock: {p.Stock})", p.Id.ToString())).ToArray();
    }
}
