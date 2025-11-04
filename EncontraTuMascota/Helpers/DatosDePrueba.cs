using EncontraTuMascota.Models;

namespace EncontraTuMascota.Helpers;

/// <summary>
/// Clase para generar datos de prueba de mascotas.
/// </summary>
public static class DatosDePrueba
{
    public const bool USAR_DATOS_DE_PRUEBA = true;

    public static List<Mascota> GenerarMascotas()
    {
        if (!USAR_DATOS_DE_PRUEBA)
            return new List<Mascota>();

        var mascotas = new List<Mascota>
        {
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1587300003388-59208cc962cb?w=400",
                Sexo = Sexo.Masculino,
                Raza = Raza.Labrador,
                Ubicacion = "Palermo, Buenos Aires",
                NombreContacto = "María González",
                TelefonoContacto = "+541145678901",
                EmailContacto = "maria.gonzalez@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-2)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1583511655857-d19b40a7a54e?w=400",
                Sexo = Sexo.Femenino,
                Raza = Raza.GoldenRetriever,
                Ubicacion = "Recoleta, Buenos Aires",
                NombreContacto = "Juan Pérez",
                TelefonoContacto = "+541156789012",
                EmailContacto = "juan.perez@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-5)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1568572933382-74d440642117?w=400",
                Sexo = Sexo.Masculino,
                Raza = Raza.PastorAleman,
                Ubicacion = "Villa Crespo, Buenos Aires",
                NombreContacto = "Ana Martínez",
                TelefonoContacto = "+541167890123",
                EmailContacto = "ana.martinez@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-1)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1543466835-00a7907e9de1?w=400",
                Sexo = Sexo.Femenino,
                Raza = Raza.Beagle,
                Ubicacion = "Belgrano, Buenos Aires",
                NombreContacto = "Carlos López",
                TelefonoContacto = "+541178901234",
                EmailContacto = "carlos.lopez@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-7)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1561037404-61cd46aa615b?w=400",
                Sexo = Sexo.Masculino,
                Raza = Raza.Bulldog,
                Ubicacion = "Caballito, Buenos Aires",
                NombreContacto = "Laura Fernández",
                TelefonoContacto = "+541189012345",
                EmailContacto = "laura.fernandez@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-3)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1552053831-71594a27632d?w=400",
                Sexo = Sexo.Femenino,
                Raza = Raza.Poodle,
                Ubicacion = "Núñez, Buenos Aires",
                NombreContacto = "Diego Romero",
                TelefonoContacto = "+541190123456",
                EmailContacto = "diego.romero@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-4)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1534361960057-19889db9621e?w=400",
                Sexo = Sexo.Masculino,
                Raza = Raza.HuskySiberiano,
                Ubicacion = "Colegiales, Buenos Aires",
                NombreContacto = "Sofía Ruiz",
                TelefonoContacto = "+541101234567",
                EmailContacto = "sofia.ruiz@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-6)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1558788353-f76d92427f16?w=400",
                Sexo = Sexo.Femenino,
                Raza = Raza.Mestizo,
                Ubicacion = "Flores, Buenos Aires",
                NombreContacto = "Pablo Sánchez",
                TelefonoContacto = "+541112345678",
                EmailContacto = "pablo.sanchez@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-8)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1555685812-4b943f1cb0eb?w=400",
                Sexo = Sexo.Masculino,
                Raza = Raza.Boxer,
                Ubicacion = "San Telmo, Buenos Aires",
                NombreContacto = "Lucía Torres",
                TelefonoContacto = "+541123456789",
                EmailContacto = "lucia.torres@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddHours(-12)
            },
            new Mascota
            {
                FotoUrl = "https://images.unsplash.com/photo-1537151608828-ea2b11777ee8?w=400",
                Sexo = Sexo.Femenino,
                Raza = Raza.Chihuahua,
                Ubicacion = "Almagro, Buenos Aires",
                NombreContacto = "Martín Castro",
                TelefonoContacto = "+541134567890",
                EmailContacto = "martin.castro@ejemplo.com",
                FechaPublicacion = DateTime.Now.AddDays(-9)
            }
        };

        return mascotas;
    }

    public static List<Publicacion> GenerarPublicaciones(List<Mascota> mascotas)
    {
        if (!USAR_DATOS_DE_PRUEBA || mascotas == null || !mascotas.Any())
            return new List<Publicacion>();

        var descripciones = new[]
        {
            "Encontré este perrito muy cariñoso cerca de la plaza. Tiene collar pero sin identificación. Se ve bien cuidado.",
            "Lo vi deambulando por la zona, parece perdido. Es muy dócil y se acerca a las personas. Necesita volver con su familia.",
            "Mascota encontrada en buen estado de salud. Estaba en la puerta de un local, parecía desorientado.",
            "Lo encontramos en el parque, es muy tranquilo y amigable. Alguien debe estar buscándolo.",
            "Apareció en la vereda de mi casa, está asustado pero no agresivo. Por favor ayuden a encontrar al dueño.",
            "Perrito muy simpático encontrado cerca de la estación. Tiene chapita identificatoria pero está desgastada.",
            "Lo vi solo en la calle, parece cachorro todavía. Está bien de salud pero necesita volver a casa.",
            "Encontrado en la zona, es muy sociable con otros perros. Claramente está acostumbrado a vivir en casa.",
            "Mascota perdida encontrada esta mañana. Responde a silbidos, debe tener familia que lo cuida.",
            "Lo encontré cerca del supermercado. Es pequeño pero muy activo. Espero que encuentren a su dueño pronto."
        };

        var publicaciones = new List<Publicacion>();

        for (int i = 0; i < mascotas.Count; i++)
        {
            var mascota = mascotas[i];
            publicaciones.Add(new Publicacion
            {
                MascotaId = mascota.Id,
                Descripcion = descripciones[i],
                Contacto = $"{mascota.NombreContacto} - Tel: {mascota.TelefonoContacto} - Email: {mascota.EmailContacto}",
                Fecha = mascota.FechaPublicacion,
                Mascota = mascota
            });
        }

        return publicaciones;
    }
}
