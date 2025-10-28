using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;
using EncontraTuMascota.Helpers;

namespace EncontraTuMascota.Controllers;

public class MascotasController : Controller
{
    // Lista temporal en memoria (después se reemplazará con BD)
    private static readonly MascotasList _mascotas = new();
    private static readonly PublicacionesList _publicaciones = new();
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MascotasController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Publicar()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publicar(Mascota mascota, string descripcion, IFormFile? foto)
    {
        // Procesar la foto si se subió
        if (foto != null && foto.Length > 0)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "mascotas");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{foto.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(fileStream);
            }

            mascota.FotoUrl = $"/uploads/mascotas/{uniqueFileName}";
        }
        else
        {
            ModelState.AddModelError("FotoUrl", "La foto es obligatoria");
        }

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
                Contacto = $"{mascota.NombreContacto} - Tel: {mascota.TelefonoContacto} - Email: {mascota.EmailContacto}",
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
                m.Ubicacion!.Contains(termino, StringComparison.OrdinalIgnoreCase));
        }

        ViewBag.Termino = termino;
        ViewBag.Publicaciones = _publicaciones;
        
        return View(mascotas.ToList());
    }
}
