using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

public class Usuario : IdentityUser
{
    [Display(Name = "Nombre Completo")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? NombreCompleto { get; set; }
    
    [Display(Name = "Fecha de Registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
    
    public ICollection<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();
}
