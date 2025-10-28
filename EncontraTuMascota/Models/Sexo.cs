using System.ComponentModel.DataAnnotations;

namespace EncontraTuMascota.Models;

public enum Sexo
{
    [Display(Name = "Masculino")]
    Masculino,
    
    [Display(Name = "Femenino")]
    Femenino
}
