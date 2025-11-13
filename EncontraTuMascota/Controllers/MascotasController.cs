using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EncontraTuMascota.Models;
using EncontraTuMascota.Helpers;
using EncontraTuMascota.Data;

namespace EncontraTuMascota.Controllers;

public class MascotasController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserManager<Usuario> _userManager;

    public MascotasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<Usuario> userManager)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _userManager = userManager;
        
        if (DatosDePrueba.USAR_DATOS_DE_PRUEBA && !_context.Mascotas.Any())
        {
            var mascotasPrueba = DatosDePrueba.GenerarMascotas();
            var publicacionesPrueba = DatosDePrueba.GenerarPublicaciones(mascotasPrueba);
            
            _context.Mascotas.AddRange(mascotasPrueba);
            _context.Publicaciones.AddRange(publicacionesPrueba);
            _context.SaveChanges();
        }
    }

    [Authorize]
    public IActionResult Publicar()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publicar(Mascota mascota, string descripcion, IFormFile? foto)
    {
        var usuario = await _userManager.GetUserAsync(User);
        
        ModelState.Remove("FotoUrl");
        
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
            ModelState.AddModelError("FotoUrl", "La foto es obligatoria. Por favor, selecciona una imagen de tu dispositivo.");
        }

        if (ModelState.IsValid)
        {
            mascota.FechaPublicacion = DateTime.Now;
            
            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();

            var publicacion = new Publicacion
            {
                MascotaId = mascota.Id,
                UsuarioId = usuario?.Id,
                Descripcion = descripcion,
                Contacto = $"{mascota.NombreContacto} - Tel: {mascota.TelefonoContacto} - Email: {mascota.EmailContacto}",
                Fecha = DateTime.Now
            };

            _context.Publicaciones.Add(publicacion);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = Messages.Success.MascotaPublicada;
            return RedirectToAction(nameof(Buscar));
        }

        ViewBag.Descripcion = descripcion;
        
        return View(mascota);
    }

    public async Task<IActionResult> Buscar(string? termino, bool sexoMasculino = false, bool sexoFemenino = false, int? raza = null, DateTime? fechaDesde = null)
    {
        try
        {
            var query = _context.Mascotas
                .Include(m => m.Publicaciones)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(termino))
            {
                query = query.Where(m => m.Ubicacion!.Contains(termino));
            }

            if (sexoMasculino || sexoFemenino)
            {
                query = query.Where(m => 
                    (sexoMasculino && m.Sexo == Sexo.Masculino) ||
                    (sexoFemenino && m.Sexo == Sexo.Femenino));
            }

            if (raza.HasValue)
            {
                query = query.Where(m => (int)m.Raza == raza.Value);
            }

            if (fechaDesde.HasValue)
            {
                query = query.Where(m => m.FechaPublicacion >= fechaDesde.Value);
            }

            var mascotas = await query.OrderByDescending(m => m.FechaPublicacion).ToListAsync();

            var publicaciones = await _context.Publicaciones
                .Include(p => p.Mascota)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.Termino = termino;
            ViewBag.SexoMasculino = sexoMasculino;
            ViewBag.SexoFemenino = sexoFemenino;
            ViewBag.RazaSeleccionada = raza;
            ViewBag.FechaDesde = fechaDesde?.ToString("yyyy-MM-dd");
            ViewBag.Publicaciones = publicaciones;
            
            return View(mascotas);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en Buscar: {ex.Message}");
            ViewBag.Publicaciones = new List<Publicacion>();
            return View(new List<Mascota>());
        }
    }
}

