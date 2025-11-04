using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EncontraTuMascota.Models;
using EncontraTuMascota.Helpers;
using EncontraTuMascota.Data;

namespace EncontraTuMascota.Controllers;

/// <summary>
/// Este es el controlador más importante del sistema.
/// Acá está toda la lógica para publicar y buscar mascotas perdidas.
/// Ahora usa Entity Framework Core para guardar todo en SQL Server.
/// </summary>
public class MascotasController : Controller
{
    // Inyectamos el contexto de la base de datos
    private readonly ApplicationDbContext _context;
    
    // Esto lo necesitamos para guardar las fotos en el servidor
    private readonly IWebHostEnvironment _webHostEnvironment;
    
    // Para obtener el usuario actual
    private readonly UserManager<Usuario> _userManager;

    public MascotasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<Usuario> userManager)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
        
        // Cargamos los datos de prueba si la BD está vacía
        if (DatosDePrueba.USAR_DATOS_DE_PRUEBA && !_context.Mascotas.Any())
        {
            var mascotasPrueba = DatosDePrueba.GenerarMascotas();
            var publicacionesPrueba = DatosDePrueba.GenerarPublicaciones(mascotasPrueba);
            
            _context.Mascotas.AddRange(mascotasPrueba);
            _context.Publicaciones.AddRange(publicacionesPrueba);
            _context.SaveChanges();
        }
    }

    // GET: /Mascotas/Publicar
    // Muestra el formulario para publicar una mascota encontrada
    // REQUIERE ESTAR LOGUEADO
    [Authorize]
    public IActionResult Publicar()
    {
        return View();
    }

    // POST: /Mascotas/Publicar
    // Acá cae toda la data cuando apretás el botón de "Publicar"
    // REQUIERE ESTAR LOGUEADO
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publicar(Mascota mascota, string descripcion, IFormFile? foto)
    {
        // Obtenemos el usuario actual
        var usuario = await _userManager.GetUserAsync(User);
        
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
            // Asignamos la fecha de publicación
            mascota.FechaPublicacion = DateTime.Now;
            
            // Agregamos la mascota a la BD (el ID se genera automáticamente)
            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();

            // Creamos la publicación que va asociada a esta mascota
            var publicacion = new Publicacion
            {
                MascotaId = mascota.Id,
                UsuarioId = usuario?.Id, // Vinculamos con el usuario actual
                Descripcion = descripcion,
                Contacto = $"{mascota.NombreContacto} - Tel: {mascota.TelefonoContacto} - Email: {mascota.EmailContacto}",
                Fecha = DateTime.Now
            };

            _context.Publicaciones.Add(publicacion);
            await _context.SaveChangesAsync();

            // Mostramos un mensaje de éxito y redirigimos a la búsqueda
            TempData["SuccessMessage"] = Messages.Success.MascotaPublicada;
            return RedirectToAction(nameof(Buscar));
        }

        // Si algo falló, volvemos al formulario con los errores
        return View(mascota);
    }

    // GET: /Mascotas/Buscar
    // Búsqueda con filtros avanzados: ubicación, sexo (checkboxes), raza y fecha
    public async Task<IActionResult> Buscar(string? termino, bool sexoMasculino = false, bool sexoFemenino = false, int? raza = null, DateTime? fechaDesde = null)
    {
        // Empezamos con todas las mascotas, incluyendo sus publicaciones
        var query = _context.Mascotas.Include(m => m.Publicaciones).AsQueryable();

        // Filtro 1: Si pusiste algo en el buscador de ubicación, filtramos
        if (!string.IsNullOrWhiteSpace(termino))
        {
            query = query.Where(m => m.Ubicacion!.Contains(termino));
        }

        // Filtro 2: Si seleccionaste sexos específicos (checkboxes)
        if (sexoMasculino || sexoFemenino)
        {
            query = query.Where(m => 
                (sexoMasculino && m.Sexo == Sexo.Masculino) ||
                (sexoFemenino && m.Sexo == Sexo.Femenino));
        }

        // Filtro 3: Si seleccionaste una raza específica
        if (raza.HasValue)
        {
            query = query.Where(m => (int)m.Raza == raza.Value);
        }

        // Filtro 4: Si pusiste una fecha "desde", filtramos mascotas publicadas desde esa fecha
        if (fechaDesde.HasValue)
        {
            query = query.Where(m => m.FechaPublicacion >= fechaDesde.Value);
        }

        // Ejecutamos la query y obtenemos los resultados
        var mascotas = await query.OrderByDescending(m => m.FechaPublicacion).ToListAsync();

        // Obtenemos todas las publicaciones para pasarlas a la vista
        var publicaciones = await _context.Publicaciones.Include(p => p.Mascota).ToListAsync();

        // Pasamos la data a la vista (incluimos los filtros aplicados para mantenerlos seleccionados)
        ViewBag.Termino = termino;
        ViewBag.SexoMasculino = sexoMasculino;
        ViewBag.SexoFemenino = sexoFemenino;
        ViewBag.RazaSeleccionada = raza;
        ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
        ViewBag.Publicaciones = publicaciones;
        
        return View(mascotas);
    }
}

