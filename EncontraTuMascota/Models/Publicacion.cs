using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EncontraTuMascota.Models;

public class Publicacion
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una mascota")]
    [Display(Name = "Mascota")]
    [ForeignKey("Mascota")]
    public int MascotaId { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [Display(Name = "Descripción")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 1000 caracteres")]
    [DataType(DataType.MultilineText)]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El contacto es obligatorio")]
    [Display(Name = "Contacto")]
    [StringLength(200, ErrorMessage = "El contacto no puede exceder 200 caracteres")]
    public string? Contacto { get; set; }

    [Display(Name = "Fecha")]
    [DataType(DataType.DateTime)]
    public DateTime Fecha { get; set; } = DateTime.Now;

    // Navegación
    public virtual Mascota? Mascota { get; set; }
}
