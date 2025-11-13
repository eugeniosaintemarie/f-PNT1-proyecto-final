using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        string[] roles = { "Admin", "Usuario" };
        
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        
        if (adminUser == null)
        {
            adminUser = new Usuario
            {
                UserName = adminEmail,
                Email = adminEmail,
                NombreCompleto = "Administrador",
                FechaRegistro = DateTime.Now,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"Γ£à Usuario admin creado: {adminEmail} / Admin123");
            }
            else
            {
                Console.WriteLine($"Γ¥î Error al crear usuario admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        
        var adminSimple = await userManager.FindByNameAsync("admin");
        if (adminSimple == null)
        {
            adminSimple = new Usuario
            {
                UserName = "admin",
                Email = "admin@sistema.com",
                NombreCompleto = "Admin",
                FechaRegistro = DateTime.Now,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminSimple, "Admin123");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminSimple, "Admin");
                Console.WriteLine($"Γ£à Usuario admin creado: admin");
            }
            else
            {
                Console.WriteLine($"Γ¥î Error al crear usuario admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        if (!context.Mascotas.Any())
        {
            var adminSimpleUser = await userManager.FindByNameAsync("admin");
            if (adminSimpleUser != null)
            {
                var mascotasPrueba = new List<Mascota>
                {
                    new Mascota
                    {
                        FotoUrl = "https://images.unsplash.com/photo-1587300003388-59208cc962cb?w=400",
                        Ubicacion = "Parque Centenario, Buenos Aires",
                        Sexo = Sexo.Masculino,
                        Raza = Raza.Labrador,
                        FechaPublicacion = DateTime.Now.AddDays(-5),
                        NombreContacto = "Juan P├⌐rez",
                        TelefonoContacto = "+541123456789",
                        EmailContacto = "juan.perez@email.com"
                    },
                    new Mascota
                    {
                        FotoUrl = "https://images.unsplash.com/photo-1583511655857-d19b40a7a54e?w=400",
                        Ubicacion = "Plaza Italia, Palermo",
                        Sexo = Sexo.Femenino,
                        Raza = Raza.GoldenRetriever,
                        FechaPublicacion = DateTime.Now.AddDays(-3),
                        NombreContacto = "Mar├¡a Gonz├ílez",
                        TelefonoContacto = "+541198765432",
                        EmailContacto = "maria.gonzalez@email.com"
                    },
                    new Mascota
                    {
                        FotoUrl = "https://images.unsplash.com/photo-1568572933382-74d440642117?w=400",
                        Ubicacion = "Recoleta, cerca del cementerio",
                        Sexo = Sexo.Masculino,
                        Raza = Raza.PastorAleman,
                        FechaPublicacion = DateTime.Now.AddDays(-7),
                        NombreContacto = "Carlos Rodr├¡guez",
                        TelefonoContacto = "+541145678901",
                        EmailContacto = "carlos.rodriguez@email.com"
                    },
                    new Mascota
                    {
                        FotoUrl = "https://images.unsplash.com/photo-1543466835-00a7907e9de1?w=400",
                        Ubicacion = "Belgrano, Av. Cabildo",
                        Sexo = Sexo.Femenino,
                        Raza = Raza.Beagle,
                        FechaPublicacion = DateTime.Now.AddDays(-2),
                        NombreContacto = "Ana Mart├¡nez",
                        TelefonoContacto = "+541156789012",
                        EmailContacto = "ana.martinez@email.com"
                    },
                    new Mascota
                    {
                        FotoUrl = "https://images.unsplash.com/photo-1561037404-61cd46aa615b?w=400",
                        Ubicacion = "Villa Urquiza, cerca de la estaci├│n",
                        Sexo = Sexo.Masculino,
                        Raza = Raza.Bulldog,
                        FechaPublicacion = DateTime.Now.AddDays(-1),
                        NombreContacto = "Pedro L├│pez",
                        TelefonoContacto = "+541167890123",
                        EmailContacto = "pedro.lopez@email.com"
                    }
                };

                await context.Mascotas.AddRangeAsync(mascotasPrueba);
                await context.SaveChangesAsync();

                var publicacionesPrueba = new List<Publicacion>();
                foreach (var mascota in mascotasPrueba)
                {
                    publicacionesPrueba.Add(new Publicacion
                    {
                        MascotaId = mascota.Id,
                        UsuarioId = adminSimpleUser.Id,
                        Fecha = mascota.FechaPublicacion,
                        Cerrada = false
                    });
                }

                await context.Publicaciones.AddRangeAsync(publicacionesPrueba);
                await context.SaveChangesAsync();

                Console.WriteLine($"Γ£à Se crearon {mascotasPrueba.Count} mascotas y publicaciones de prueba para el usuario admin");
            }
        }
    }
}
