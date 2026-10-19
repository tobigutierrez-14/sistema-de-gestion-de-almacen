using Microsoft.AspNetCore.Mvc;
using SistemaGestion.Web.Services;

namespace SistemaGestion.Web.Controllers;

public sealed class HomeController(CatalogoEnMemoria catalogo) : Controller
{
    public IActionResult Index() => View(catalogo.ResumenDashboard());
}
