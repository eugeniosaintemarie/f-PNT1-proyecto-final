using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [Display(Name = "Correo Electrónico")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
    public string? Email { get; set; }
}
