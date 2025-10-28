using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Controllers;

/// <summary>
/// Controlador del home. Lo más básico que hay.
/// Sólo muestra la página principal con los dos botones grandes.
/// </summary>
public class HomeController : Controller
{
    // La página de inicio, nada del otro mundo
    public IActionResult Index()
    {
        return View();
    }
}
