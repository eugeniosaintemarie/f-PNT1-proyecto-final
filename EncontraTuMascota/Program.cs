// Configuración inicial del proyecto ASP.NET Core MVC
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EncontraTuMascota.Data;
using EncontraTuMascota.Models;

// Fix para WebRootPath cuando se ejecuta desde bin/Debug
var currentDir = Directory.GetCurrentDirectory();
string contentRoot = currentDir;
string webRoot = Path.Combine(currentDir, "wwwroot");

if (currentDir.Contains("bin"))
{
    // Navegar hacia arriba para encontrar el directorio del proyecto
    contentRoot = Path.GetFullPath(Path.Combine(currentDir, "..", "..", ".."));
    webRoot = Path.Combine(contentRoot, "wwwroot");
}

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = contentRoot,
    WebRootPath = webRoot
});

builder.WebHost.UseUrls("http://127.0.0.1:5006");

// Configuramos Entity Framework Core con SQL Server
// La connection string está acá mismo por simplicidad, en producción irían en appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        "Server=(localdb)\\mssqllocaldb;Database=EncontraTuMascotaDB;Trusted_Connection=true;MultipleActiveResultSets=true",
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

// Configuramos ASP.NET Identity para gestión de usuarios
builder.Services.AddIdentity<Usuario, IdentityRole>(options => 
{
    // Configuración de contraseñas (puedes ajustar según tus necesidades)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false; // No requerimos mayúscula para "admin"
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5; // Mínimo 5 para permitir "admin"
    
    // Configuración de usuario
    options.User.RequireUniqueEmail = false; // Permitir usuarios sin email (como "admin")
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    
    // Configuración de lockout (bloqueo de cuenta)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configurar las cookies de autenticación
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/"; // Redirigir al home en lugar de a una vista de Login
    options.AccessDeniedPath = "/"; // Redirigir al home si acceso denegado
    options.SlidingExpiration = true;
});

// Agregamos soporte para Controllers y Views (MVC)
builder.Services.AddControllersWithViews();

// Habilitamos sesiones (por compatibilidad, aunque Identity usa cookies)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Aplicamos migraciones pendientes automáticamente al arrancar
// (en producción esto NO se hace así, pero para desarrollo está bien)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        // Aplicar migraciones
        dbContext.Database.Migrate();
        Console.WriteLine("✅ Migraciones aplicadas correctamente");
        
        // Inicializar roles y usuario admin
        await DbInitializer.Initialize(services);
        Console.WriteLine("✅ Base de datos inicializada correctamente");
    }
    catch (Exception ex)
    {
        // Si falla, logueamos el error pero seguimos
        Console.WriteLine($"❌ Error al inicializar BD: {ex.Message}");
    }
}

// Configuración del pipeline de requests HTTP
// (el orden importa, no lo cambies a lo loco)
if (!app.Environment.IsDevelopment())
{
  // Si estamos en producción, usamos páginas de error custom
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// No redirección a HTTPS en Development
app.UseStaticFiles();
app.UseRouting();

// Habilita sesiones (por compatibilidad)
app.UseSession();

// IMPORTANTE: Authentication debe ir ANTES de Authorization
app.UseAuthentication();
app.UseAuthorization();

// Ruta por defecto: Home/Index
// Esto significa que si entrás a la raíz del sitio, te manda al HomeController, acción Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Arranca el server
app.Run();
