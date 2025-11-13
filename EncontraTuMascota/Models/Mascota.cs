using System.ComponentModel.DataAnnotations;
using EncontraTuMascota.Helpers;

namespace EncontraTuMascota.Models;

public class Mascota
{
    [Key]
    public int Id { get; set; }

    [Display(Name = "Foto")]
    public string? FotoUrl { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio")]
    [Display(Name = "Sexo")]
    public Sexo Sexo { get; set; }

    [Required(ErrorMessage = "La raza es obligatoria")]
    [Display(Name = "Raza")]
    public Raza Raza { get; set; }

    [Required(ErrorMessage = "La ubicación es obligatoria")]
    [Display(Name = "Ubicación donde se encontró")]
    [StringLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres")]
    public string? Ubicacion { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? NombreContacto { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Display(Name = "Teléfono")]
    [TelefonoArgentino]
    public string? TelefonoContacto { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Debe ser un email válido")]
    [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
    public string? EmailContacto { get; set; }

    [Display(Name = "Fecha de Publicación")]
    [DataType(DataType.DateTime)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    public virtual ICollection<Publicacion>? Publicaciones { get; set; }
}
