using Microsoft.AspNetCore.Mvc;
using SistemaGestion.Web.Services;

namespace SistemaGestion.Web.Controllers;

public sealed class HomeController(CatalogoEnMemoria catalogo, VentasEnMemoria ventas) : Controller
{
    public IActionResult Index()
    {
        var modelo = catalogo.ResumenDashboard();
        var hoy = ventas.Hoy();
        modelo.VentasDelDia = hoy.Cantidad;
        modelo.IngresosDelDia = hoy.Importe;
        return View(modelo);
    }
}
