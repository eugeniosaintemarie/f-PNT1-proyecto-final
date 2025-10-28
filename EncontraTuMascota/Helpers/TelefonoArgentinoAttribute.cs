using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EncontraTuMascota.Helpers;

public class TelefonoArgentinoAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success; // El [Required] maneja esto
        }

        string telefono = value.ToString()!.Trim();
        
        // Patrón para teléfono argentino: +54 seguido de código de área y número
        // Ejemplos válidos: +5411, +54911, +54351, etc.
        var regex = new Regex(@"^\+54(9?\d{2,4})\d{6,8}$");
        
        if (!regex.IsMatch(telefono))
        {
            return new ValidationResult("El teléfono debe ser de Argentina (formato: +5411XXXXXXXX o +54911XXXXXXXX)");
        }

        return ValidationResult.Success;
    }
}
