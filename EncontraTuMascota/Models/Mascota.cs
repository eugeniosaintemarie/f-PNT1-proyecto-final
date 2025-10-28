using System.ComponentModel.DataAnnotations;
using EncontraTuMascota.Helpers;

namespace EncontraTuMascota.Models;

/// <summary>
/// Acá guardamos toda la data de las mascotas que se encuentran en la calle.
/// La idea es que cualquiera que se cruce con un perrito o michito perdido
/// pueda publicarlo para que el dueño lo encuentre.
/// </summary>
public class Mascota
{
    // El ID se auto-genera, no te preocupes por esto
    [Key]
    public int Id { get; set; }

    // La foto es RE importante, sin foto nadie va a reconocer a su mascota
    [Required(ErrorMessage = "La foto es obligatoria")]
    [Display(Name = "Foto")]
    public string? FotoUrl { get; set; }

    // Macho o hembra, básico pero necesario para filtrar búsquedas
    [Required(ErrorMessage = "El sexo es obligatorio")]
    [Display(Name = "Sexo")]
    public Sexo Sexo { get; set; }

    // La raza ayuda banda a identificar, aunque muchos son mestizos y está todo bien
    [Required(ErrorMessage = "La raza es obligatoria")]
    [Display(Name = "Raza")]
    public Raza Raza { get; set; }

    // Dónde lo encontraste? Una dirección, un barrio, una esquina, lo que sea
    [Required(ErrorMessage = "La ubicación es obligatoria")]
    [Display(Name = "Ubicación donde se encontró")]
    [StringLength(200, ErrorMessage = "La ubicación no puede exceder 200 caracteres")]
    public string? Ubicacion { get; set; }

    // Tu nombre para que te puedan contactar si es la mascota que buscan
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [Display(Name = "Nombre")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
    public string? NombreContacto { get; set; }

    // Teléfono argentino (tiene que empezar con +54, sino no te lo acepta)
    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Display(Name = "Teléfono")]
    [TelefonoArgentino]
    public string? TelefonoContacto { get; set; }

    // Email de contacto por si prefieren mandarte un mail
    [Required(ErrorMessage = "El email es obligatorio")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Debe ser un email válido")]
    [StringLength(150, ErrorMessage = "El email no puede exceder 150 caracteres")]
    public string? EmailContacto { get; set; }

    // Se guarda automáticamente cuando publicas
    [Display(Name = "Fecha de Publicación")]
    [DataType(DataType.DateTime)]
    public DateTime FechaPublicacion { get; set; } = DateTime.Now;

    // Una mascota puede tener varias publicaciones (si se la ve en distintos lugares)
    public virtual ICollection<Publicacion>? Publicaciones { get; set; }
}
