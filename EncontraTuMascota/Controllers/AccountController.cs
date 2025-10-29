using Microsoft.AspNetCore.Mvc;

namespace EncontraTuMascota.Controllers;

/// <summary>
/// Controlador para manejar autenticación (login/logout).
/// Por ahora es super básico con usuario/contraseña admin/admin.
/// Después esto se va a conectar con Identity y base de datos.
/// </summary>
public class AccountController : Controller
{
    // POST: /Account/Login
    // Maneja el inicio de sesión
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Validación simple: admin/admin
        if (username == "admin" && password == "admin")
        {
            // Guardamos el usuario en la sesión
            HttpContext.Session.SetString("Usuario", username);
            return Ok();
        }
        
        return Unauthorized();
    }

    // POST: /Account/Logout
    // Cierra la sesión
    [HttpPost]
    public IActionResult Logout()
    {
        // Limpiamos la sesión
        HttpContext.Session.Clear();
        return Ok();
    }
}
