// Configuración inicial del proyecto ASP.NET Core MVC

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

// Agregamos soporte para Controllers y Views (MVC)
builder.Services.AddControllersWithViews();

// Habilitamos sesiones para el sistema de login
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // La sesión dura 30 minutos
    options.Cookie.HttpOnly = true; // Por seguridad
    options.Cookie.IsEssential = true; // Necesario para GDPR
});

var app = builder.Build();

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

// Habilita sesiones (tiene que ir antes de UseAuthorization)
app.UseSession();

// Habilita autorización (por ahora no lo usamos pero va a venir bien después)
app.UseAuthorization();

// Ruta por defecto: Home/Index
// Esto significa que si entrás a la raíz del sitio, te manda al HomeController, acción Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Arranca el server
app.Run();
