using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

/// <summary>
/// Usuario de la aplicación.
/// Extiende IdentityUser para agregar campos personalizados.
/// IdentityUser ya incluye: Id, UserName, Email, PasswordHash, PhoneNumber, etc.
/// </summary>
public class Usuario : IdentityUser
{
    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    [Display(Name = "Nombre Completo")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? NombreCompleto { get; set; }
    
    /// <summary>
    /// Fecha de registro en la plataforma
    /// </summary>
    [Display(Name = "Fecha de Registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Publicaciones realizadas por este usuario
    /// </summary>
    public ICollection<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();
}
