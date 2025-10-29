// Configuración inicial del proyecto ASP.NET Core MVC
var builder = WebApplication.CreateBuilder(args);

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
}

// Redirección automática a HTTPS (por seguridad)
app.UseHttpsRedirection();

// Habilita archivos estáticos (CSS, JS, imágenes, etc. de wwwroot)
app.UseStaticFiles();

// Habilita el routing
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
