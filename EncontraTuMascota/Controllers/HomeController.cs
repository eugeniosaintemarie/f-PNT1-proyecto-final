using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
