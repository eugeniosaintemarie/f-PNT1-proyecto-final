using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;
using EncontraTuMascota.Helpers;

namespace EncontraTuMascota.Controllers;

public class MascotasController : Controller
{
    // Lista temporal en memoria (después se reemplazará con BD)
    private static readonly MascotasList _mascotas = new();
    private static readonly PublicacionesList _publicaciones = new();

    public IActionResult Publicar()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Publicar(Mascota mascota, string descripcion, string contacto)
    {
        if (ModelState.IsValid)
        {
            // Asignar ID autoincremental
            mascota.Id = _mascotas.Count > 0 ? _mascotas.Max(m => m.Id) + 1 : 1;
            mascota.FechaPublicacion = DateTime.Now;
            
            _mascotas.Add(mascota);

            // Crear la publicación asociada
            var publicacion = new Publicacion
            {
                Id = _publicaciones.Count > 0 ? _publicaciones.Max(p => p.Id) + 1 : 1,
                MascotaId = mascota.Id,
                Descripcion = descripcion,
                Contacto = contacto,
                Fecha = DateTime.Now,
                Mascota = mascota
            };

            _publicaciones.Add(publicacion);

            TempData["SuccessMessage"] = Messages.Success.MascotaPublicada;
            return RedirectToAction(nameof(Buscar));
        }

        return View(mascota);
    }

    public IActionResult Buscar(string? termino)
    {
        var mascotas = _mascotas.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(termino))
        {
            mascotas = mascotas.Where(m => 
                m.CalleEncontrada!.Contains(termino, StringComparison.OrdinalIgnoreCase));
        }

        ViewBag.Termino = termino;
        ViewBag.Publicaciones = _publicaciones;
        
        return View(mascotas.ToList());
    }
}
