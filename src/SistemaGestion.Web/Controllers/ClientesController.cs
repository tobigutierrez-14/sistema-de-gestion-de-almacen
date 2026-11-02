using Microsoft.AspNetCore.Mvc;
using SistemaGestion.Web.Services;
using SistemaGestion.Web.ViewModels;

namespace SistemaGestion.Web.Controllers;

public sealed class ClientesController(ClientesEnMemoria clientes) : Controller
{
    [HttpGet]
    public IActionResult Index(string? busqueda)
    {
        ViewData["Busqueda"] = busqueda ?? "";
        return View(clientes.Listar(busqueda));
    }

    [HttpGet]
    public IActionResult Crear() => View("Formulario", new ClienteFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(ClienteFormViewModel datos)
    {
        ValidarDni(datos);
        if (!ModelState.IsValid) return View("Formulario", datos);
        try { clientes.Agregar(datos); }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Formulario", datos);
        }
        TempData["Exito"] = "Cliente registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var datos = clientes.Obtener(id);
        return datos is null ? NotFound() : View("Formulario", datos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int id, ClienteFormViewModel datos)
    {
        var original = clientes.Obtener(id);
        if (original is null) return NotFound();
        datos.Id = id;
        // No confiar en un DNI enviado desde el navegador: usar el original.
        ModelState.Remove(nameof(datos.Dni));
        datos.Dni = original.Dni;
        if (!ModelState.IsValid) return View("Formulario", datos);
        try
        {
            if (!clientes.Actualizar(id, datos)) return NotFound();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Formulario", datos);
        }
        TempData["Exito"] = "Cliente actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private void ValidarDni(ClienteFormViewModel datos)
    {
        if (string.IsNullOrWhiteSpace(datos.Dni)) return;
        if (clientes.DniOcupado(datos.Dni))
            ModelState.AddModelError(nameof(datos.Dni), "Ya existe un cliente con ese DNI.");
    }
}
