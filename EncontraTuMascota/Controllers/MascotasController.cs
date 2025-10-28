using Microsoft.AspNetCore.Mvc;
using EncontraTuMascota.Models;
using EncontraTuMascota.Helpers;

namespace EncontraTuMascota.Controllers;

/// <summary>
/// Este es el controlador más importante del sistema.
/// Acá está toda la lógica para publicar y buscar mascotas perdidas.
/// Por ahora guarda todo en memoria, pero después lo vamos a conectar a una BD.
/// </summary>
public class MascotasController : Controller
{
    // Listas en memoria (temporal hasta que tengamos Entity Framework funcionando)
    // AVISO: Si reiniciás el server se borra todo, es normal por ahora
    private static readonly MascotasList _mascotas = new();
    private static readonly PublicacionesList _publicaciones = new();
    
    // Esto lo necesitamos para guardar las fotos en el servidor
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MascotasController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: /Mascotas/Publicar
    // Muestra el formulario para publicar una mascota encontrada
    public IActionResult Publicar()
    {
        return View();
    }

    // POST: /Mascotas/Publicar
    // Acá cae toda la data cuando apretás el botón de "Publicar"
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publicar(Mascota mascota, string descripcion, IFormFile? foto)
    {
        // Primero che queamos si subió una foto (es obligatoria, sino ¿cómo la va a reconocer?)
        if (foto != null && foto.Length > 0)
        {
            // Creamos la carpeta donde van las fotos si no existe
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "mascotas");
            Directory.CreateDirectory(uploadsFolder);

            // Le ponemos un nombre único a la foto (GUID + nombre original)
            // Así no se pisan entre ellas si suben dos fotos con el mismo nombre
            var uniqueFileName = $"{Guid.NewGuid()}_{foto.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Guardamos la foto en el disco
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await foto.CopyToAsync(fileStream);
            }

            // Guardamos la ruta para mostrarla después
            mascota.FotoUrl = $"/uploads/mascotas/{uniqueFileName}";
        }
        else
        {
            // Si no subió foto, le tiramos error
            ModelState.AddModelError("FotoUrl", "La foto es obligatoria");
        }

        // Si pasó todas las validaciones, guardamos la mascota
        if (ModelState.IsValid)
        {
            // Le asignamos un ID (por ahora manual, después lo hace la BD)
            mascota.Id = _mascotas.Count > 0 ? _mascotas.Max(m => m.Id) + 1 : 1;
            mascota.FechaPublicacion = DateTime.Now;
            
            // Agregamos a la lista
            _mascotas.Add(mascota);

            // Creamos la publicación que va asociada a esta mascota
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

            // Mostramos un mensaje de éxito y redirigimos a la búsqueda
            TempData["SuccessMessage"] = Messages.Success.MascotaPublicada;
            return RedirectToAction(nameof(Buscar));
        }

        // Si algo falló, volvemos al formulario con los errores
        return View(mascota);
    }

    // GET: /Mascotas/Buscar
    // Acá podés buscar mascotas por ubicación (barrio, calle, lo que sea)
    public IActionResult Buscar(string? termino)
    {
        // Empezamos con todas las mascotas
        var mascotas = _mascotas.AsEnumerable();

        // Si pusiste algo en el buscador, filtramos
        if (!string.IsNullOrWhiteSpace(termino))
        {
            mascotas = mascotas.Where(m => 
                m.Ubicacion!.Contains(termino, StringComparison.OrdinalIgnoreCase));
        }

        // Pasamos la data a la vista
        ViewBag.Termino = termino;
        ViewBag.Publicaciones = _publicaciones;
        
        return View(mascotas.ToList());
    }
}
