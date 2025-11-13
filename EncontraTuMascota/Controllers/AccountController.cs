using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EncontraTuMascota.Models;
using EncontraTuMascota.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace EncontraTuMascota.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return Redirect("/");
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe = false)
    {
        var result = await _signInManager.PasswordSignInAsync(
            username, 
            password, 
            rememberMe, 
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return Ok();
        }
        
        if (result.IsLockedOut)
        {
            return StatusCode(403, "Tu cuenta está bloqueada temporalmente por múltiples intentos fallidos.");
        }

        return Unauthorized();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return Redirect("/");
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Register(string nombreCompleto, string username, string? email, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            return BadRequest("Las contraseñas no coinciden");
        }

        var existingUser = await _userManager.FindByNameAsync(username);
        if (existingUser != null)
        {
            return BadRequest("El nombre de usuario ya está en uso");
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            var existingEmail = await _userManager.FindByEmailAsync(email);
            if (existingEmail != null)
            {
                return BadRequest("El email ya está registrado");
            }
        }

        var usuario = new Usuario
        {
            UserName = username,
            Email = string.IsNullOrWhiteSpace(email) ? null : email,
            NombreCompleto = nombreCompleto,
            FechaRegistro = DateTime.Now,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(usuario, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(usuario, "Usuario");
            
            await _signInManager.SignInAsync(usuario, isPersistent: false);
            return Ok();
        }

        var errorMessages = result.Errors.Select(e => 
        {
            if (e.Code == "DuplicateUserName") return "El nombre de usuario ya existe";
            if (e.Code == "DuplicateEmail") return "El email ya está registrado";
            if (e.Code == "PasswordTooShort") return "La contraseña es muy corta";
            if (e.Code == "PasswordRequiresDigit") return "La contraseña debe tener al menos un número";
            if (e.Code == "PasswordRequiresLower") return "La contraseña debe tener al menos una letra minúscula";
            return e.Description;
        });
        
        var errors = string.Join(". ", errorMessages);
        return BadRequest(errors);
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    public IActionResult AccessDenied()
    {
        return Redirect("/");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> MisPublicaciones()
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario == null)
        {
            return Redirect("/");
        }

        var publicaciones = await _context.Publicaciones
            .Include(p => p.Mascota)
            .Where(p => p.UsuarioId == usuario.Id)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        ViewBag.NombreUsuario = usuario.UserName;
        return View(publicaciones);
    }

    [Authorize]
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> CerrarPublicacion(int id, string resolucion)
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario == null)
        {
            return Unauthorized();
        }

        var publicacion = await _context.Publicaciones
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuario.Id);

        if (publicacion == null)
        {
            return NotFound("Publicación no encontrada o no tienes permisos para cerrarla");
        }

        if (string.IsNullOrWhiteSpace(resolucion))
        {
            return BadRequest("Debes escribir cómo se resolvió el caso");
        }

        publicacion.Cerrada = true;
        publicacion.FechaCierre = DateTime.Now;
        publicacion.Resolucion = resolucion;

        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> EditarPublicacion(int id)
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario == null)
        {
            return Redirect("/");
        }

        var publicacion = await _context.Publicaciones
            .Include(p => p.Mascota)
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuario.Id);

        if (publicacion == null)
        {
            TempData["Error"] = "Publicación no encontrada o no tienes permisos para editarla";
            return RedirectToAction("MisPublicaciones");
        }

        if (publicacion.Cerrada)
        {
            TempData["Error"] = "No se pueden editar publicaciones cerradas";
            return RedirectToAction("MisPublicaciones");
        }

        return View(publicacion);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarPublicacion(int id, Publicacion model)
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario == null)
        {
            return Redirect("/");
        }

        var publicacion = await _context.Publicaciones
            .Include(p => p.Mascota)
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuario.Id);

        if (publicacion == null)
        {
            TempData["Error"] = "Publicación no encontrada o no tienes permisos para editarla";
            return RedirectToAction("MisPublicaciones");
        }

        if (publicacion.Cerrada)
        {
            TempData["Error"] = "No se pueden editar publicaciones cerradas";
            return RedirectToAction("MisPublicaciones");
        }

        if (publicacion.Mascota == null)
        {
            TempData["Error"] = "Error al cargar los datos de la mascota";
            return RedirectToAction("MisPublicaciones");
        }

        publicacion.Mascota.Ubicacion = model.Mascota?.Ubicacion ?? publicacion.Mascota.Ubicacion;
        publicacion.Mascota.Sexo = model.Mascota?.Sexo ?? publicacion.Mascota.Sexo;
        publicacion.Mascota.Raza = model.Mascota?.Raza ?? publicacion.Mascota.Raza;
        publicacion.Mascota.NombreContacto = model.Mascota?.NombreContacto ?? publicacion.Mascota.NombreContacto;
        publicacion.Mascota.TelefonoContacto = model.Mascota?.TelefonoContacto ?? publicacion.Mascota.TelefonoContacto;
        publicacion.Mascota.EmailContacto = model.Mascota?.EmailContacto;
        
        if (!string.IsNullOrWhiteSpace(model.Mascota?.FotoUrl))
        {
            publicacion.Mascota.FotoUrl = model.Mascota.FotoUrl;
        }

        if (!string.IsNullOrWhiteSpace(model.Descripcion))
        {
            publicacion.Descripcion = model.Descripcion;
        }

        await _context.SaveChangesAsync();

        TempData["Mensaje"] = "Publicación actualizada exitosamente";
        return RedirectToAction("MisPublicaciones");
    }

    [Authorize]
    [HttpPost]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> EliminarPublicacion(int id)
    {
        var usuario = await _userManager.GetUserAsync(User);
        if (usuario == null)
        {
            return Unauthorized();
        }

        var publicacion = await _context.Publicaciones
            .Include(p => p.Mascota)
            .FirstOrDefaultAsync(p => p.Id == id && p.UsuarioId == usuario.Id);

        if (publicacion == null)
        {
            return NotFound("Publicación no encontrada o no tienes permisos para eliminarla");
        }

        if (publicacion.Mascota == null)
        {
            return BadRequest("Error: mascota no encontrada");
        }

        _context.Mascotas.Remove(publicacion.Mascota);
        await _context.SaveChangesAsync();

        return Ok();
    }
}

// ViewModels para Login y Registro
public class LoginViewModel
{
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recordarme")]
    public bool RememberMe { get; set; }
}

public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    [Display(Name = "Nombre Completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Confirmar Contraseña")]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
