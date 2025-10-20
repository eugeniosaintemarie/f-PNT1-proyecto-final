namespace EncontraTuMascota.Helpers;

public static class DateTimeHelper
{
    public static string ToFriendlyString(this DateTime date)
    {
        var timeSpan = DateTime.Now - date;

        if (timeSpan.TotalMinutes < 1)
            return "Hace un momento";
        
        if (timeSpan.TotalMinutes < 60)
            return $"Hace {(int)timeSpan.TotalMinutes} minuto{((int)timeSpan.TotalMinutes > 1 ? "s" : "")}";
        
        if (timeSpan.TotalHours < 24)
            return $"Hace {(int)timeSpan.TotalHours} hora{((int)timeSpan.TotalHours > 1 ? "s" : "")}";
        
        if (timeSpan.TotalDays < 7)
            return $"Hace {(int)timeSpan.TotalDays} día{((int)timeSpan.TotalDays > 1 ? "s" : "")}";
        
        return date.ToString("dd/MM/yyyy");
    }
}
