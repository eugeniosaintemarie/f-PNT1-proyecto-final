using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EncontraTuMascota.Helpers;

/// <summary>
/// Validación custom para teléfonos argentinos.
/// Tiene que venir en formato internacional (+54) sino no pasa.
/// Ejemplos que andan: +5411XXXXXXXX, +54911XXXXXXXX, +543515XXXXXX
/// </summary>
public class TelefonoArgentinoAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Si está vacío, que lo valide el [Required], no es problema nuestro
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        string telefono = value.ToString()!.Trim();
        
        // Patrón para validar teléfonos argentinos:
        // +54 (código de país) + código de área (2 a 4 dígitos, puede tener 9 adelante para celu)
        // seguido de 6 a 8 dígitos más
        var regex = new Regex(@"^\+54(9?\d{2,4})\d{6,8}$");
        
        if (!regex.IsMatch(telefono))
        {
            return new ValidationResult("El teléfono debe ser de Argentina (formato: +5411XXXXXXXX o +54911XXXXXXXX)");
        }

        return ValidationResult.Success;
    }
}
