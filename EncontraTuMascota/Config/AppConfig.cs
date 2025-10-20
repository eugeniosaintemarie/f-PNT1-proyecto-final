namespace EncontraTuMascota.Config;

public static class AppConfig
{
    // Configuración de la aplicación
    public const string AppName = "Encontrá Tu Mascota";
    public const string AppVersion = "1.0.0";
    
    // Configuración de archivos
    public static class Files
    {
        public const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        public const string DefaultImagePath = "/images/default-pet.png";
    }
    
    // Configuración de paginación
    public static class Pagination
    {
        public const int DefaultPageSize = 10;
        public const int MaxPageSize = 50;
    }
}
