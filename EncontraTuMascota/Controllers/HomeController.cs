using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Controllers;

/// <summary>
/// Controlador del home
/// Sólo muestra la página principal con los dos botones grandes.
/// </summary>
public class HomeController : Controller
{
    // La página de inicio
    public IActionResult Index()
    {
        return View();
    }
}
