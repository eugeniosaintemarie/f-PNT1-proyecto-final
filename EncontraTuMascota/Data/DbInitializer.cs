using Microsoft.AspNetCore.Identity;
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
    }
}