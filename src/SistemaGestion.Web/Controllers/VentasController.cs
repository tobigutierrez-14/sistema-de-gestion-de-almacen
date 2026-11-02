using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaGestion.Web.Services;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Controllers;

public sealed class VentasController(VentasEnMemoria ventas, CatalogoEnMemoria catalogo,
    ClientesEnMemoria clientes) : Controller
{
    [HttpGet]
    public IActionResult Index() => View(ventas.Listar());

    [HttpGet]
    public IActionResult Crear()
    {
        PrepararFormulario();
        return View(new VentaFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(VentaFormViewModel datos)
    {
        if (datos.Lineas is null || !datos.Lineas.Any(x => x.ProductoId != 0 && x.Cantidad > 0))
            ModelState.AddModelError(string.Empty, "Seleccioná al menos un producto con cantidad mayor a cero.");
        if (!ModelState.IsValid)
        {
            PrepararFormulario();
            return View(datos);
        }
        try
        {
            var id = ventas.CrearYConfirmar(datos);
            TempData["Exito"] = $"Venta N.º {id} confirmada correctamente.";
            return RedirectToAction(nameof(Detalle), new { id });
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PrepararFormulario();
            return View(datos);
        }
    }

    [HttpGet]
    public IActionResult Detalle(int id)
    {
        var venta = ventas.Obtener(id);
        return venta is null ? NotFound() : View(venta);
    }

    private void PrepararFormulario()
    {
        ViewData["Clientes"] = clientes.OpcionesVentas().Select(c =>
            new SelectListItem($"{c.Nombre} · DNI {c.Dni}", c.Id.ToString())).ToList();
        ViewData["Medios"] = ventas.MediosPago().Select(m =>
            new SelectListItem(m.Nombre, m.Id.ToString())).ToList();
        ViewData["ProductosCatalogo"] = catalogo.Productos();
    }
}
