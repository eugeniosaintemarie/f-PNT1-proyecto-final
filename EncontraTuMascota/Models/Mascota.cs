using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

public class Mascota
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "La URL de la foto es obligatoria")]
    [Display(Name = "Foto de la Mascota")]
    [DataType(DataType.ImageUrl)]
    public string? FotoUrl { get; set; }

    [Required(ErrorMessage = "La calle donde se encontró es obligatoria")]
    [Display(Name = "Calle Encontrada")]
    [StringLength(200, ErrorMessage = "La calle no puede exceder 200 caracteres")]
    public string? CalleEncontrada { get; set; }

    [Display(Name = "Fecha de Publicación")]
    [DataType(DataType.DateTime)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    // Relación con Publicaciones
    public virtual ICollection<Publicacion>? Publicaciones { get; set; }
}
