using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

/// <summary>
/// Simple: Macho o Hembra. Nada más que eso.
/// </summary>
public enum Sexo
{
    [Display(Name = "Masculino")]
    Masculino,
    
    [Display(Name = "Femenino")]
    Femenino
}
