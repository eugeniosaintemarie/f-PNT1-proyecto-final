# Guia Unidad 5 - MVC

## Tutorial basico MVC ASP.NET Core (PDF)
- Explica la arquitectura MVC: separacion entre Modelo (datos y reglas de negocio), Vista (interfaz y renderizado) y Controlador (flujo y coordinacion).
- Describe los prerequisitos para crear un proyecto *ASP.NET Core Web Application* con la plantilla MVC en Visual Studio 2017/2019.
- Paso a paso para armar el dominio del ejemplo: clase `Estudiante` con atributos y data annotations, enum `Deporte`, contexto `EscuelaDatabaseContext` con `DbSet<Estudiante>`.
- Configuracion de `appsettings.json` con la cadena de conexion a SQL Server (`EscuelaDB`).
- Instalacion de paquetes NuGet (`Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.AspNetCore.Mvc.NewtonsoftJson`) segun la version del SDK.
- Ajustes en `Startup.cs` para registrar el contexto, habilitar MVC y evitar referencias circulares en JSON. Incluye variantes para MVC 2.1 y 3.1.
- Uso de la Consola de Paquetes para ejecutar `Add-Migration` y `Update-Database`, generando la tabla `Estudiantes` con EF Core Code First.
- Scaffold del `EstudianteController` con vistas CRUD (`Index`, `Details`, `Create`, `Edit`, `Delete`).
- Personalizacion de vistas: agregar el enlace en `_Layout` y poblar el `<select>` de deportes con `Html.GetEnumSelectList<Deporte>()`.
- Verificacion final ejecutando la aplicacion y probando el CRUD sobre la base de datos.

## Proyectos de ejemplo en la carpeta

### `1 MVC basico terminado 3.0`
- Fragmentos del proyecto **BilleteraVirtualMVC** orientado a .NET Core 3.0; incluye `Startup.cs` migrado al patron `AddControllersWithViews` y registro del `BilleteraContext` con SQL Server.
- `appsettings.json` define la cadena `DefaultConnection` hacia `BilleteraVirtualDB` utilizando `SQLExpress` con `MultipleActiveResultSets` habilitado.
- Se agrega configuracion JSON con `ReferenceLoopHandling.Ignore` mediante `services.AddControllers().AddNewtonsoftJson(...)`.
- Archivo `version nueva de newtonsoft.txt` recuerda compatibilidad 3.1 y la extension de paquetes necesarios; `Paquetes.PNG` captura la configuracion de NuGet.
- Carpeta pensada como referencia para ajustes de compatibilidad cuando se migra el proyecto 2.1 a 3.0 o superior.

### `2 MVC basico terminado 2.1`
- Solucion **MVCBasico.sln** lista para .NET Core 2.1 con estructura completa de un CRUD de estudiantes.
- `Program.cs` usa `WebHost.CreateDefaultBuilder` y `Startup` con pipeline clasico (`UseMvc`).
- `appsettings.json` y `appsettings.Development.json` guardan la cadena `ConnectionString:EscuelaDBConnection` apuntando a `SQLExpress`.
- `Context/EscuelaDatabaseContext.cs` registra `DbSet<Estudiante>`; las migraciones crean la tabla `Estudiantes` con columnas `Nombre`, `Edad`, `FechaInscripto`, `DeporteFavorito`.
- `Models` incluye `Estudiante` con data annotations, enum `Deporte` y `ErrorViewModel`.
- `Controllers/EstudianteController.cs` contiene las acciones generadas por scaffolding para CRUD asincrono; `HomeController` mantiene vistas informativas.
- Vistas Razor bajo `Views/Estudiante` generan formularios Bootstrap. La version 2.1 deja el `<select>` de `DeporteFavorito` sin `asp-items`, permitiendo practicar la carga del enum segun el tutorial.
- `_Layout.cshtml` (Bootstrap 3) agrega navegacion hacia `Estudiante` y mantiene el pie de pagina 2020.
- `wwwroot` conserva los assets por defecto y `Migrations/` almacena el snapshot para EF Core 2.1.

### `3 MVC clasico terminado 3.1`
- Proyecto **MVCClasico** enfocado en .NET Core 3.1 con la plantilla MVC moderna (`CreateHostBuilder`, endpoint routing, Bootstrap 4).
- `Startup.cs` configura cookies, registra `EscuelaDatabaseContext` y agrega MVC con `AddNewtonsoftJson` mas `SetCompatibilityVersion(CompatibilityVersion.Version_3_0)` para conservar comportamiento clasico.
- `appsettings.json` define `ConnectionString:EscuelaDBConnection` apuntando a un servidor local `MSSQLSERVER01`.
- `Program.cs` adopta el Generic Host mientras mantiene el namespace `MVCBasico` para compartir modelos y contexto con la version 2.1.
- `Controllers`, `Models` y `Context` replican la logica CRUD del ejemplo 2.1, pero actualizan atributos (como data annotations en una sola linea y comentarios al dia).
- Vistas actualizadas a Bootstrap 4: `_Layout.cshtml` usa el nuevo menu responsive y `Estudiante/Create` ya incorpora `asp-items="Html.GetEnumSelectList<Deporte>()"` con una opcion predeterminada.
- `Migrations` re-generadas con EF Core 3.1 (identidad `1, 1`) y `EscuelaDatabaseContextModelSnapshot.cs` correspondiente.
- Proyecto sirve como referencia de migracion 2.1 -> 3.1, mostrando los cambios mas relevantes en pipeline, vistas y compatibilidad JSON.

## Notas finales
- Todos los proyectos asumen SQL Server LocalDB/Express y requieren actualizar la cadena de conexion a la instancia disponible en cada entorno.
- Para extender el ejemplo puede agregarse validacion en `Estudiante`, campos adicionales o nuevas entidades relacionadas.
- El tutorial PDF complementa los proyectos guiando la creacion desde cero; las carpetas 2 y 3 permiten comparar las diferencias entre versiones de .NET Core.
