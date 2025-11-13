namespace EncontraTuMascota.Helpers;

public static class Messages
{
    public static class Success
    {
        public const string MascotaPublicada = "¡Mascota publicada exitosamente!";
        public const string ContactoEnviado = "¡Mensaje enviado correctamente!";
        public const string OperacionExitosa = "Operación realizada con éxito";
    }

    public static class Error
    {
        public const string ErrorGeneral = "Ocurrió un error. Por favor, intente nuevamente.";
        public const string CamposRequeridos = "Por favor complete todos los campos requeridos.";
        public const string ErrorGuardar = "Error al guardar la información.";
        public const string NoEncontrado = "No se encontró el recurso solicitado.";
    }

    public static class Validation
    {
        public const string EmailInvalido = "El formato del correo electrónico no es válido.";
        public const string CampoRequerido = "Este campo es obligatorio.";
    }
}
