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
    
    // Flag para saber si ya cargamos los datos de prueba
    private static bool _datosDePruebaCargados = false;
    
    // Esto lo necesitamos para guardar las fotos en el servidor
    private readonly IWebHostEnvironment _webHostEnvironment;

    public MascotasController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
        
        // Cargamos los datos de prueba la primera vez que se instancia el controller
        if (!_datosDePruebaCargados && DatosDePrueba.USAR_DATOS_DE_PRUEBA)
        {
            var mascotasPrueba = DatosDePrueba.GenerarMascotas();
            var publicacionesPrueba = DatosDePrueba.GenerarPublicaciones(mascotasPrueba);
            
            _mascotas.AddRange(mascotasPrueba);
            _publicaciones.AddRange(publicacionesPrueba);
            
            _datosDePruebaCargados = true;
        }
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
        // Primero chequeamos si subió una foto (es obligatoria, sino ¿cómo la va a reconocer?)
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
    // Búsqueda con filtros avanzados: ubicación, sexo (checkboxes), raza y fecha
    public IActionResult Buscar(string? termino, bool sexoMasculino = false, bool sexoFemenino = false, int? raza = null, DateTime? fechaDesde = null)
    {
        // Empezamos con todas las mascotas
        var mascotas = _mascotas.AsEnumerable();

        // Filtro 1: Si pusiste algo en el buscador de ubicación, filtramos
        if (!string.IsNullOrWhiteSpace(termino))
        {
            mascotas = mascotas.Where(m => 
                m.Ubicacion!.Contains(termino, StringComparison.OrdinalIgnoreCase));
        }

        // Filtro 2: Si seleccionaste sexos específicos (checkboxes)
        if (sexoMasculino || sexoFemenino)
        {
            mascotas = mascotas.Where(m => 
                (sexoMasculino && m.Sexo == Sexo.Masculino) ||
                (sexoFemenino && m.Sexo == Sexo.Femenino));
        }

        // Filtro 3: Si seleccionaste una raza específica
        if (raza.HasValue)
        {
            mascotas = mascotas.Where(m => (int)m.Raza == raza.Value);
        }

        // Filtro 4: Si pusiste una fecha "desde", filtramos mascotas publicadas desde esa fecha
        if (fechaDesde.HasValue)
        {
            mascotas = mascotas.Where(m => m.FechaPublicacion >= fechaDesde.Value);
        }

        // Pasamos la data a la vista (incluimos los filtros aplicados para mantenerlos seleccionados)
        ViewBag.Termino = termino;
        ViewBag.SexoMasculino = sexoMasculino;
        ViewBag.SexoFemenino = sexoFemenino;
        ViewBag.RazaSeleccionada = raza;
        ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
        ViewBag.Publicaciones = _publicaciones;
        
        return View(mascotas.ToList());
    }
}
