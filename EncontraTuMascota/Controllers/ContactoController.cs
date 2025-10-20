using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;
using EncontraTuMascota.Helpers;

namespace EncontraTuMascota.Controllers;

public class ContactoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(Usuario usuario, string mensaje)
    {
        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(mensaje))
        {
            // Aquí se procesaría el envío del mensaje (email, BD, etc.)
            // Por ahora solo mostramos mensaje de éxito
            
            TempData["SuccessMessage"] = Messages.Success.ContactoEnviado;
            return RedirectToAction("Index", "Home");
        }

        if (string.IsNullOrWhiteSpace(mensaje))
        {
            ModelState.AddModelError("mensaje", "El mensaje es obligatorio");
        }

        return View(usuario);
    }
}
