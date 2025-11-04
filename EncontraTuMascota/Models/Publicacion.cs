using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EncontraTuMascota.Models;

/// <summary>
/// Cada vez que alguien publica una mascota, se crea esto.
/// Es como el "anuncio" que aparece en la búsqueda.
/// </summary>
public class Publicacion
{
    // ID único de la publicación
    [Key]
    public int Id { get; set; }

    // A qué mascota corresponde esta publicación (una mascota puede tener varias)
    [Required(ErrorMessage = "Debe seleccionar una mascota")]
    [Display(Name = "Mascota")]
    [ForeignKey("Mascota")]
    public int MascotaId { get; set; }

    // Contá un poco más: cómo la encontraste, si tiene collar, si está lastimada, etc.
    [Required(ErrorMessage = "La descripción es obligatoria")]
    [Display(Name = "Descripción")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 1000 caracteres")]
    [DataType(DataType.MultilineText)]
    public string? Descripcion { get; set; }

    // Info de contacto (se arma automáticamente con nombre, teléfono y email)
    [Required(ErrorMessage = "El contacto es obligatorio")]
    [Display(Name = "Contacto")]
    [StringLength(200, ErrorMessage = "El contacto no puede exceder 200 caracteres")]
    public string? Contacto { get; set; }

    // Cuándo se publicó (se guarda solo, no lo toques)
    [Display(Name = "Fecha")]
    [DataType(DataType.DateTime)]
    public DateTime Fecha { get; set; } = DateTime.Now;

    // Usuario que creó esta publicación (opcional por ahora para compatibilidad)
    [Display(Name = "Usuario")]
    public string? UsuarioId { get; set; }

    // Esto es para que EF Core pueda navegar entre Publicacion y Mascota
    public virtual Mascota? Mascota { get; set; }
    
    // Navegación al usuario que creó la publicación
    [ForeignKey("UsuarioId")]
    public virtual Usuario? Usuario { get; set; }
}