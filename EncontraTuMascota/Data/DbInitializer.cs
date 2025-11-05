using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Data;

/// <summary>
/// Inicializador de la base de datos.
/// Crea roles y usuario administrador por defecto.
/// </summary>
public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Crear roles si no existen
        string[] roles = { "Admin", "Usuario" };
        
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Crear usuario admin si no existe
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
                EmailConfirmed = true // Auto-confirmar email del admin
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123");
            
            if (result.Succeeded)
            {
                // Asignar rol Admin
                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine($"✅ Usuario admin creado: {adminEmail} / Admin123");
            }
            else
            {
                Console.WriteLine($"❌ Error al crear usuario admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        
        // Crear usuario admin simple (admin/Admin1) si no existe
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

            var result = await userManager.CreateAsync(adminSimple, "Admin1");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminSimple, "Admin");
                Console.WriteLine($"✅ Usuario admin creado: admin / Admin1");
            }
            else
            {
                Console.WriteLine($"❌ Error al crear usuario admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Crear mascotas y publicaciones de prueba si no existen
        if (!context.Mascotas.Any())
        {
            var adminSimpleUser = await userManager.FindByNameAsync("admin");
            if (adminSimpleUser != null)
            {
                var mascotasPrueba = new List<Mascota>
                {
                    new Mascota
                    {
                        Ubicacion = "Parque Centenario, Buenos Aires",
                        Sexo = Sexo.Masculino,
                        Raza = Raza.Labrador,
                        FechaPublicacion = DateTime.Now.AddDays(-5),
                        NombreContacto = "Juan Pérez",
                        TelefonoContacto = "+541123456789",
                        EmailContacto = "juan.perez@email.com"
                    },
                    new Mascota
                    {
                        Ubicacion = "Plaza Italia, Palermo",
                        Sexo = Sexo.Femenino,
                        Raza = Raza.GoldenRetriever,
                        FechaPublicacion = DateTime.Now.AddDays(-3),
                        NombreContacto = "María González",
                        TelefonoContacto = "+541198765432",
                        EmailContacto = "maria.gonzalez@email.com"
                    },
                    new Mascota
                    {
                        Ubicacion = "Recoleta, cerca del cementerio",
                        Sexo = Sexo.Masculino,
                        Raza = Raza.PastorAleman,
                        FechaPublicacion = DateTime.Now.AddDays(-7),
                        NombreContacto = "Carlos Rodríguez",
                        TelefonoContacto = "+541145678901",
                        EmailContacto = "carlos.rodriguez@email.com"
                    },
                    new Mascota
                    {
                        Ubicacion = "Belgrano, Av. Cabildo",
                        Sexo = Sexo.Femenino,
                        Raza = Raza.Beagle,
                        FechaPublicacion = DateTime.Now.AddDays(-2),
                        NombreContacto = "Ana Martínez",
                        TelefonoContacto = "+541156789012",
                        EmailContacto = "ana.martinez@email.com"
                    },
                    new Mascota
                    {
                        Ubicacion = "Villa Urquiza, cerca de la estación",
                        Sexo = Sexo.Masculino,
                        Raza = Raza.Bulldog,
                        FechaPublicacion = DateTime.Now.AddDays(-1),
                        NombreContacto = "Pedro López",
                        TelefonoContacto = "+541167890123",
                        EmailContacto = "pedro.lopez@email.com"
                    }
                };

                await context.Mascotas.AddRangeAsync(mascotasPrueba);
                await context.SaveChangesAsync();

                // Crear publicaciones para cada mascota
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

                Console.WriteLine($"✅ Se crearon {mascotasPrueba.Count} mascotas y publicaciones de prueba para el usuario admin");
            }
        }
    }}
