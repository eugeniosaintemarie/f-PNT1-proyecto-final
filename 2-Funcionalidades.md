# 2. Funcionalidades del Sistema "Encuentra Tu Mascota"# Encontrá Tu Mascota - Documentación de Funcionalidades

**Aplicación Web ASP.NET Core MVC 8.0**  

## Visión General

## 📋 Índice

"Encuentra Tu Mascota" es una aplicación web ASP.NET Core MVC que conecta personas que han perdido o encontrado mascotas. El sistema permite publicar casos, buscar con filtros avanzados, gestionar publicaciones propias y contactar a otros usuarios de forma segura.1. [Resumen del Sistema](#resumen-del-sistema)

2. [Arquitectura y Tecnologías](#arquitectura-y-tecnologías)

### Stack Tecnológico3. [Modelos de Datos](#modelos-de-datos)

- **Backend**: ASP.NET Core 8.0 MVC + Entity Framework Core 8.04. [Funcionalidades Principales](#funcionalidades-principales)

- **Autenticación**: ASP.NET Core Identity con roles (Admin, Usuario)5. [Controladores](#controladores)

- **Base de Datos**: SQL Server LocalDB6. [Vistas y UI](#vistas-y-ui)

- **Frontend**: Razor Views, Bootstrap 5, JavaScript/jQuery7. [Sistema de Autenticación](#sistema-de-autenticación)

- **Validaciones**: Data Annotations + Custom Validators8. [Base de Datos](#base-de-datos)

9. [Helpers y Utilidades](#helpers-y-utilidades)

---10. [Validaciones](#validaciones)



## F1: Sistema de Autenticación y Roles## 🎯 Resumen del Sistema

**Encontrá Tu Mascota** es una plataforma web colaborativa diseñada para reunir mascotas perdidas con sus familias. Permite a usuarios registrados publicar mascotas encontradas en la vía pública y facilita la búsqueda mediante filtros avanzados.

### ¿Qué hace?

Permite a los usuarios registrarse, iniciar sesión y gestionar su cuenta. Incluye un sistema de roles que diferencia entre usuarios comunes y administradores.### Características Principales:

- 🔍 **Búsqueda pública** de mascotas con filtros avanzados

### Archivos involucrados:- 📝 **Publicación** de mascotas encontradas (requiere autenticación)

- `Controllers/AccountController.cs`- 🔒 **Protección de datos** de contacto con sistema de blur

- `Views/Account/Login.cshtml`, `Register.cshtml`- 👥 **Sistema de usuarios** con roles (Admin, Usuario)

- `Views/Shared/_Layout.cshtml` (modal de login/registro)- 📱 **Interfaz responsive** con Material Design

- `Models/Usuario.cs`

- `Helpers/DatosDePrueba.cs` (inicialización de roles)## 🏗️ Arquitectura y Tecnologías



### ¿Para qué sirve?### Framework y Versiones

- **Seguridad**: Solo usuarios autenticados pueden publicar mascotas- **ASP.NET Core MVC**: 8.0

- **Gestión**: Cada usuario gestiona solo sus propias publicaciones- **Entity Framework Core**: 8.0.0

- **Administración**: El rol Admin tiene permisos especiales- **ASP.NET Core Identity**: 8.0.0

- **UX**: Modal de login AJAX sin recargar página- **Base de Datos**: SQL Server LocalDB



### Flujo:### Patrón Arquitectónico

1. Usuario accede desde cualquier página- **MVC (Model-View-Controller)**: Separación de responsabilidades

2. Click en "Iniciar Sesión" abre modal- **Repository Pattern**: A través de DbContext

3. Credenciales se validan vía AJAX- **Dependency Injection**: Configurado en Program.cs

4. Si es exitoso, se actualiza UI sin reload

5. Usuario redirigido a página protegida si corresponde### Paquetes NuGet Principales

```xml

---- Microsoft.EntityFrameworkCore.SqlServer (8.0.0)

- Microsoft.EntityFrameworkCore.Tools (8.0.0)

## F2: Publicación de Mascotas Perdidas/Encontradas- Microsoft.EntityFrameworkCore.Design (8.0.0)

- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.0)

### ¿Qué hace?```

Permite a usuarios autenticados reportar mascotas perdidas o encontradas mediante un formulario completo con validaciones.

## 📊 Modelos de Datos

### Archivos involucrados:

- `Controllers/MascotasController.cs` (acción `Publicar()`)### 1. Usuario (Identity)

- `Views/Mascotas/Publicar.cshtml`Extiende `IdentityUser` de ASP.NET Core Identity.

- `Models/Mascota.cs`, `Publicacion.cs`**Propiedades:**

- `Helpers/TelefonoArgentinoAttribute.cs` (validación personalizada)```csharp

- Id: string (heredado, PK)

### ¿Para qué sirve?- UserName: string (heredado, único)

- **Reportar casos**: Usuarios ingresan detalles de mascotas perdidas/encontradas- Email: string (heredado, opcional)

- **Validación**: Campos obligatorios (foto, email, teléfono argentino)- PasswordHash: string (heredado)

- **Trazabilidad**: Cada publicación queda vinculada al usuario que la creó- NombreCompleto: string? (custom)

- **Información completa**: Descripción, ubicación, fecha, contacto, raza, sexo- FechaRegistro: DateTime (custom)

- Publicaciones: ICollection<Publicacion> (navigation property)

### Campos requeridos:```

- **Foto** (obligatorio): IFormFile subido a `wwwroot/uploads/mascotas/`**Funcionalidad:**

- **Email de contacto** (obligatorio): Para comunicación- Gestiona la autenticación y autorización

- **Teléfono**: Validación de formato argentino- Almacena información de perfil del usuario

- **Descripción**: Detalles del caso- Relaciona usuarios con sus publicaciones

- **Raza y Sexo**: Enums para estandarizar

- **Ubicación y Fecha**: Contexto del avistamiento### 2. Mascota

**Propiedades:**

### Flujo:```csharp

1. Usuario autenticado accede a "Publicar Mascota"- Id: int (PK, auto-incremental)

2. Completa formulario con datos y foto- Sexo: Sexo (enum: Masculino, Femenino)

3. JavaScript muestra preview de foto seleccionada- Raza: Raza (enum: 10 razas disponibles)

4. POST valida campos (ModelState.Remove para FotoUrl)- FotoUrl: string (URL de imagen, **requerido**)

5. Foto se guarda en servidor con nombre único- Ubicacion: string (requerido)

6. Entidades Mascota y Publicacion se crean enlazadas- FechaPublicacion: DateTime

7. Redirect a "Mis Publicaciones"- NombreContacto: string (requerido)

- TelefonoContacto: string (requerido, formato argentino)

---- EmailContacto: string (email de contacto, **requerido**)

- Publicaciones: ICollection<Publicacion> (navigation property)

## F3: Búsqueda y Filtrado de Publicaciones```

**Validaciones:**

### ¿Qué hace?- `[Required]` en campos obligatorios

Proporciona un motor de búsqueda con múltiples filtros para encontrar mascotas por texto, raza, sexo y fecha.- `[StringLength]` para límites de caracteres

- `[TelefonoArgentino]` custom validator para teléfonos

### Archivos involucrados:- `[EmailAddress]` para formato de email

- `Controllers/MascotasController.cs` (acción `Buscar()`)**Funcionalidad:**

- `Views/Mascotas/Buscar.cshtml`- Representa mascotas encontradas en la vía pública

- `Models/Sexo.cs`, `Raza.cs` (enums)- Almacena datos descriptivos y de contacto

- Se relaciona 1:N con Publicaciones

### ¿Para qué sirve?

- **Búsqueda flexible**: Texto libre busca en descripción, ubicación, nombre y contacto### 3. Publicacion

- **Filtros**: Raza (dropdown), Sexo (checkboxes), Fecha (date picker)**Propiedades:**

- **Resultados ordenados**: Por fecha de publicación descendente```csharp

- **Sin autenticación**: Accesible para todos los usuarios- Id: int (PK, auto-incremental)

- MascotaId: int (FK, requerido)

### Filtros disponibles:- Mascota: Mascota (navigation property)

1. **Texto**: Búsqueda en múltiples campos (LIKE en SQL)- UsuarioId: string? (FK, nullable)

2. **Raza**: Dropdown con valores del enum Raza- Usuario: Usuario? (navigation property)

3. **Sexo**: Checkboxes Macho/Hembra- Descripcion: string? (opcional)

4. **Fecha desde**: Date picker para rango temporal- Contacto: string? (opcional)

- Fecha: DateTime

### Flujo:- Cerrada: bool (default: false)

1. Usuario accede a "Buscar Mascota"- FechaCierre: DateTime? (nullable)

2. Aplica filtros deseados- Resolucion: string? (max 500 caracteres, nullable)

3. GET con query strings a `Buscar()````

4. LINQ aplica filtros dinámicos**Validaciones:**

5. Vista renderiza cards con fotos y datos- `[Display]` para nombres amigables

6. Click en card muestra detalles completos- `[StringLength(500)]` en Resolucion

- `[DataType(DataType.MultilineText)]` para Resolucion

---**Funcionalidad:**

- Vincula mascotas con usuarios que las publican

## F4: Gestión de Publicaciones Propias- Almacena información adicional de contexto

- Fecha de publicación para ordenamiento

### ¿Qué hace?- Sistema de cierre de casos con seguimiento de resolución

Permite a cada usuario ver, editar, cerrar y eliminar sus propias publicaciones.

### 4. Enumeraciones

### Archivos involucrados:

- `Controllers/AccountController.cs` (acciones `MisPublicaciones()`, `EditarPublicacion()`, `CerrarPublicacion()`, `EliminarPublicacion()`)#### Sexo

- `Views/Account/MisPublicaciones.cshtml````csharp

- `Views/Account/EditarPublicacion.cshtml`public enum Sexo

{

### ¿Para qué sirve?    Masculino,

- **Control total**: Usuario gestiona solo sus publicaciones    Femenino

- **Actualizar**: Editar datos si hubo cambios (ej. nueva ubicación)}

- **Cerrar caso**: Marcar publicación como resuelta con descripción```

- **Eliminar**: Borrar publicaciones por error o duplicadas

#### Raza

### Operaciones disponibles:```csharp

public enum Raza

#### **Ver Mis Publicaciones**{

- Lista ordenada por fecha descendente    Labrador,

- Muestra estado: ABIERTA (verde) o CERRADA (gris)    GoldenRetriever,

- Botones contextuales según estado    PastorAleman,

    Bulldog,

#### **Editar**    Beagle,

- Solo disponible para publicaciones abiertas    Poodle,

- Formulario pre-llenado con datos actuales    YorkshireTerrier,

- Validaciones iguales a publicación nueva    Chihuahua,

- Foto actual se mantiene si no se cambia    HuskySiberiano,

    CockerSpaniel

#### **Cerrar Caso**}

- Prompt JavaScript solicita resolución```

- POST marca `Cerrada = true`, registra `FechaCierre` y `Resolucion`

- Publicación ya no aparece en búsquedas activas## 🎮 Funcionalidades Principales

- Útil cuando mascota fue encontrada/devuelta

### F1: Visualización Pública de Mascotas

#### **Eliminar****Descripción:**  

- Confirmación JavaScript antes de borrarCualquier visitante (autenticado o no) puede buscar mascotas publicadas con filtros avanzados.

- Cascade delete borra Publicacion y Mascota**Características:**

- Archivo de foto NO se elimina del servidor (por seguridad)- ✅ Acceso sin autenticación

- ✅ Filtros múltiples combinables:

### Flujo:  - 📍 Ubicación (búsqueda por texto)

1. Usuario autenticado accede a "Mis Publicaciones"  - ♀️♂️ Sexo (masculino/femenino)

2. Ve lista de sus casos (abiertas y cerradas)  - 🐕 Raza (selector dropdown)

3. Según estado, ve botones:  - 📅 Fecha desde

   - **Abierta**: Editar, Cerrar, Eliminar- ✅ Ordenamiento descendente por fecha de publicación

   - **Cerrada**: Solo Eliminar- ✅ Vista en tarjetas (cards) responsive

4. Al editar: formulario precargado, puede cambiar foto- ✅ Contador de mascotas encontradas

5. Al cerrar: prompt de resolución, marca como cerrada- 🔒 **Datos de contacto con blur** para usuarios no autenticados

6. Al eliminar: confirmación, borrado permanente- ✅ Mensaje invitando a iniciar sesión para ver contactos

**Flujo:**

---1. Usuario accede a `/Mascotas/Buscar`

2. Sistema carga todas las mascotas publicadas

## F5: Contacto3. Aplica filtros si se proporcionan

4. Renderiza tarjetas con información

### ¿Qué hace?5. Si NO está autenticado: muestra contactos con efecto blur

Formulario de contacto genérico para comunicación con administradores del sitio.6. Si SÍ está autenticado: muestra contactos legibles

**Implementación Técnica:**

### Archivos involucrados:- **Controller:** `MascotasController.Buscar()`

- `Controllers/ContactoController.cs`- **View:** `Buscar.cshtml`

- `Views/Contacto/Index.cshtml`- **LINQ:** Queries con `Where()`, `OrderByDescending()`, `Include()`

- **Razor:** Condicional `@if (User.Identity?.IsAuthenticated)`

### ¿Para qué sirve?

- **Soporte**: Usuarios reportan problemas o consultas### F2: Registro de Usuarios

- **Sin base de datos**: Información se procesa pero no se persiste (placeholder)**Descripción:**  

- **Accesible**: No requiere autenticaciónPermite crear cuentas de usuario para acceder a funcionalidades autenticadas.

**Características:**

---- ✅ Popup modal para mejor UX

- ✅ Validación de unicidad de username

## F6: Panel de Inicio- ✅ Validación de unicidad de email (si se proporciona)

- ✅ Validación de contraseña en cliente y servidor

### ¿Qué hace?- ✅ Auto-login después del registro exitoso

Landing page con información del sitio y navegación principal.- ✅ Asignación automática del rol "Usuario"

- ✅ Mensajes de error traducidos al español

### Archivos involucrados:- ✅ Confirmación de contraseña

- `Controllers/HomeController.cs`**Campos del Formulario:**

- `Views/Home/Index.cshtml`- **Nombre Completo** (requerido)

- **Nombre de Usuario** (requerido, único)

### ¿Para qué sirve?- **Email** (opcional)

- **Bienvenida**: Presenta el propósito del sitio- **Contraseña** (requerido, min 5 caracteres, minúscula + dígito)

- **Navegación**: Links a buscar, publicar y contacto- **Confirmar Contraseña** (debe coincidir)

- **Pública**: Acceso sin autenticación**Validaciones de Contraseña:**

```csharp

---- RequireDigit = true (al menos un número)

- RequireLowercase = true (al menos una minúscula)

## Arquitectura de Archivos- RequireUppercase = false (mayúscula opcional)

- RequiredLength = 5 (mínimo 5 caracteres)

### **Models/** (Entidades y Lógica de Negocio)```

- `Usuario.cs`: Extiende IdentityUser con campos personalizados (NombreCompleto, FechaRegistro)**Flujo:**

- `Mascota.cs`: Datos de la mascota (foto, ubicación, raza, sexo, contacto)1. Usuario hace click en "Registrarse"

- `Publicacion.cs`: Vincula mascota con usuario, maneja estado (Cerrada, Resolucion)2. Se abre popup modal con formulario

- `Sexo.cs`: Enum (Macho, Hembra)3. Completa datos y envía (AJAX)

- `Raza.cs`: Enum con 10 razas comunes4. Backend verifica unicidad de username/email

5. Valida requisitos de contraseña

### **Controllers/** (Lógica de Controladores)6. Crea usuario en BD

- `HomeController.cs`: Página de inicio7. Asigna rol "Usuario"

- `MascotasController.cs`: Publicar y buscar mascotas8. Inicia sesión automáticamente

- `AccountController.cs`: Autenticación y gestión de publicaciones propias9. Cierra popup y recarga página

- `ContactoController.cs`: Formulario de contacto**Implementación Técnica:**

- **Controller:** `AccountController.Register()`

### **Views/** (Interfaz de Usuario)- **View:** Modal en `_Layout.cshtml`

- **Shared/**- **JavaScript:** `handleRegister()` con AJAX

  - `_Layout.cshtml`: Plantilla principal con navbar y modal de login- **Identity:** `UserManager<Usuario>.CreateAsync()`

  - `_LoginPartial.cshtml`: UI de autenticación (botones login/logout)

- **Home/**: Landing page### F3: Inicio de Sesión (Login)

- **Mascotas/**: Publicar y buscar**Descripción:**  

- **Account/**: Login, register, mis publicaciones, editarAutenticación de usuarios registrados mediante username y contraseña.

- **Contacto/**: Formulario de contacto**Características:**

- ✅ Popup modal para mejor UX

### **Helpers/** (Utilidades)- ✅ Login basado en username (no email)

- `TelefonoArgentinoAttribute.cs`: Validador personalizado de teléfonos argentinos- ✅ Opción "Recordarme" (persistent cookie)

- `Messages.cs`: Mensajes de error/éxito centralizados- ✅ Bloqueo temporal tras intentos fallidos

- `DatosDePrueba.cs`: Seed data para desarrollo (10 publicaciones de prueba)- ✅ Redirección inteligente post-login

- ✅ Mensajes de error claros

### **Data/** (Acceso a Datos)- ✅ AJAX sin recarga de página

- `ApplicationDbContext.cs`: DbContext con DbSets de Usuario, Mascota, Publicacion**Campos del Formulario:**

- `DbInitializer.cs`: Inicializa roles (Admin, Usuario) y usuario admin- **Nombre de Usuario** (requerido)

- **Contraseña** (requerido)

### **Migrations/** (Migraciones de Base de Datos)- **Recordarme** (checkbox opcional)

- `InitialCreate`: Tablas Mascotas y Publicaciones**Flujo:**

- `AddIdentity`: Integra ASP.NET Core Identity1. Usuario hace click en "Acceder" o "Iniciar Sesión"

- `AddPublicacionCerrada`: Agrega campos Cerrada, FechaCierre, Resolucion2. Se abre popup modal

3. Ingresa credenciales y envía (AJAX)

---4. Backend valida con Identity

5. Si es exitoso: crea cookie de autenticación

## Flujos de Usuario Principales6. Si vino desde "Publicar mascota": redirige allí

7. Si no: recarga página actual

### 1. Usuario nuevo publica mascota perdida**Implementación Técnica:**

1. Accede al sitio → "Publicar Mascota"- **Controller:** `AccountController.Login()`

2. Modal de login aparece (no autenticado)- **View:** Modal en `_Layout.cshtml`

3. Click en "Registrarse" → completa formulario- **JavaScript:** `handleLogin()`, `mostrarLoginConRedireccion()`

4. Después de registro, redirect a "Publicar Mascota"- **Identity:** `SignInManager<Usuario>.PasswordSignInAsync()`

5. Completa formulario con foto y detalles- **Redirect Logic:** Variable global `redirectAfterLogin`

6. Submit → mascota publicada

7. Redirect a "Mis Publicaciones"### F4: Publicar Mascota

**Descripción:**  

### 2. Usuario busca mascota encontradaPermite a usuarios autenticados publicar mascotas encontradas.

1. Accede al sitio → "Buscar Mascota"**Características:**

2. Aplica filtros: texto "golden", raza "Golden Retriever"- 🔒 **Requiere autenticación** (atributo `[Authorize]`)

3. Ve resultados con fotos- ✅ Formulario con validaciones client-side y server-side

4. Click en card para ver contacto y descripción completa- ✅ Carga de foto (URL)

5. Llama/envía email al contacto desde los datos visibles- ✅ Asociación automática con usuario actual

- ✅ Mensaje de éxito con redirección

### 3. Usuario cierra caso exitoso- ✅ Popup de advertencia si intenta acceder sin login

1. Login → "Mis Publicaciones"**Campos del Formulario:**

2. Ve su publicación abierta- **Foto (URL)** (**requerido**)

3. Click en "Cerrar Caso"- **Ubicación** (requerido)

4. Ingresa resolución: "Encontrada en parque, devuelta a dueño"- **Sexo** (radio buttons: Masculino/Femenino)

5. Publicación se marca como cerrada (cambia a gris)- **Raza** (selector dropdown)

6. Ya no aparece en búsquedas activas- **Descripción** (textarea, opcional)

- **Nombre de contacto** (requerido)

### 4. Administrador gestiona contenido- **Teléfono de Contacto** (requerido, validación especial)

1. Login con usuario admin (admin@test.com / Admin123!)- **Email de contacto** (**requerido**)

2. Puede ver todas las publicaciones en "Buscar"**Validaciones Especiales:**

3. Accede a "Mis Publicaciones" → ve 10 publicaciones de prueba- Teléfono con formato argentino (custom attribute)

4. Puede editar/cerrar/eliminar cualquiera de las suyas- Todos los campos con validación HTML5

- Validación de modelo en servidor

---**Flujo (Usuario Autenticado):**

1. Usuario hace click en "Publicar mascota"

## Aspectos Técnicos Destacados2. Sistema verifica autenticación

3. Renderiza formulario

### Seguridad4. Usuario completa datos y envía

- **Autorización**: Atributo `[Authorize]` en acciones sensibles5. Backend valida datos

- **Validación**: Server-side en POST + client-side con unobtrusive validation6. Crea entidad Mascota

- **Roles**: Admin y Usuario con permisos diferenciados7. Crea entidad Publicacion vinculada

- **Password hashing**: Manejado por ASP.NET Core Identity8. Asocia UsuarioId del usuario actual

9. Guarda en BD

### Performance10. Redirige a Buscar con mensaje de éxito

- **Eager loading**: Include() para evitar N+1 queries**Flujo (Usuario NO Autenticado):**

- **Archivos estáticos**: Fotos servidas desde wwwroot1. Usuario hace click en "Publicar mascota"

- **Validaciones tempranas**: ModelState valida antes de queries DB2. JavaScript detecta falta de autenticación

3. Muestra popup de advertencia

### UX4. Usuario hace click en "Iniciar Sesión"

- **Modal AJAX**: Login/registro sin reload5. Se abre modal de login con redirect flag

- **Preview de foto**: JavaScript muestra archivo antes de upload6. Tras login exitoso: redirige a `/Mascotas/Publicar`

- **Validación en tiempo real**: jQuery Validation Unobtrusive**Implementación Técnica:**

- **Mensajes claros**: TempData comunica éxito/error entre requests- **Controller:** `MascotasController.Publicar()` (GET y POST)

- **View:** `Publicar.cshtml`

### Validaciones Personalizadas- **Authorization:** `[Authorize]` attribute

- **TelefonoArgentinoAttribute**: Regex para formato argentino (ej. 11-1234-5678)- **JavaScript:** `mostrarAvisoAuth()` para usuarios no auth

- **Required en foto y email**: Atributos HTML5 + server-side check- **Identity:** `UserManager<Usuario>.GetUserAsync(User)`

- **Enums**: Restringen valores de Raza y Sexo

### F5: Cierre de Sesión (Logout)

---**Descripción:**  

Permite a usuarios autenticados cerrar su sesión.

## Cumplimiento del Checklist (0-Checklist.md)**Características:**

- ✅ AJAX sin recarga de página

### ✅ Requerimientos de Arquitectura- ✅ Limpia todas las cookies de Identity

- **MVC**: Controllers, Models, Views correctamente separados- ✅ Recarga página para actualizar UI

- **Entity Framework Core**: ApplicationDbContext + Migrations**Flujo:**

- **Identity**: Autenticación con roles Admin y Usuario1. Usuario hace click en "Salir"

- **Validaciones**: Data Annotations + Custom Attribute2. JavaScript envía POST a `/Account/Logout` (AJAX)

3. Backend limpia sesión con SignInManager

### ✅ Funcionalidades Obligatorias4. Retorna OK

- **F1 (Auth)**: Login, registro, logout funcionales5. Cliente recarga página

- **F2 (Publicar)**: Formulario completo con validaciones6. UI muestra estado no autenticado

- **F3 (Buscar)**: Filtros múltiples y búsqueda por texto**Implementación Técnica:**

- **F4 (Gestionar)**: Editar, cerrar, eliminar publicaciones- **Controller:** `AccountController.Logout()`

- **JavaScript:** `cerrarSesion()` en `_Layout.cshtml`

### ✅ Características Adicionales- **Identity:** `SignInManager<Usuario>.SignOutAsync()`

- **Seed data**: DatosDePrueba.cs con 10 casos de prueba

- **Roles**: Admin tiene publicaciones precargadas### F6: Protección de Datos de Contacto

- **Estado de casos**: Campo `Cerrada` en Publicacion**Descripción:**  

- **Resolución**: Campo de texto para describir cierreSistema de privacidad que oculta datos sensibles a usuarios no autenticados.

**Características:**

---- 🔒 Datos con efecto visual blur para no autenticados

- ✅ Mensaje invitando a iniciar sesión

## Notas de Implementación- ✅ Revelación completa para usuarios autenticados

- ✅ Link directo al modal de login

### Base de Datos**Datos Protegidos:**

- **LocalDB**: (localdb)\mssqllocaldb- Nombre de contacto

- **Nombre**: EncontraTuMascotaDB- Teléfono de contacto

- **Migraciones**: Aplicadas automáticamente al iniciar (DbInitializer)- Email de contacto

**Implementación Visual:**

### Usuarios de Prueba```css

- **Admin**: admin@test.com / Admin123!.contacto-blur {

- **Usuario**: usuario@test.com / Usuario123!    filter: blur(5px);

    user-select: none;

### Archivos Subidos    pointer-events: none;

- **Ruta**: wwwroot/uploads/mascotas/}

- **Formato**: {Guid}-{filename}.{ext}```

- **Tipos permitidos**: .jpg, .jpeg, .png, .gif (validación client-side recomendada)**Implementación Técnica:**

- **Razor Conditional:** `@if (User.Identity?.IsAuthenticated)`

### Próximas Mejoras Sugeridas- **CSS:** Clase `.contacto-blur`

1. Validación client-side de tipos de archivo en foto- **View:** `Buscar.cshtml`

2. Compresión de imágenes antes de subir

3. Paginación en búsqueda y "Mis Publicaciones"### F7: Panel de Usuario - Mis Publicaciones

4. Notificaciones por email cuando hay match de búsqueda**Descripción:**  

5. Administración centralizada para rol Admin (ver todas las publicaciones)Panel personal donde usuarios autenticados pueden ver y gestionar sus publicaciones.

**Características:**

---- 🔒 **Requiere autenticación**

- ✅ Lista todas las publicaciones del usuario actual

**Última actualización**: Este documento refleja el estado del proyecto con todas las funcionalidades implementadas y validadas.- ✅ Ordenadas por fecha descendente (más recientes primero)

- ✅ Información completa de cada publicación:
  - Ubicación
  - Sexo, Raza y Fecha
  - Estado (Abierta/Cerrada)
- ✅ **Gestión completa de publicaciones:**
  - ✏️ Editar publicaciones abiertas
  - ✓ Cerrar publicaciones activas
  - 🗑️ Eliminar cualquier publicación
- ✅ Modal para registrar resolución del caso
- ✅ Visualización de resolución en casos cerrados
- ✅ Diseño responsive con layout de 3 columnas
- ✅ Validaciones de seguridad (solo propietario puede gestionar)
**Acceso:**
- Click en el nombre de usuario en la barra de navegación
- URL directa: `/Account/MisPublicaciones`
**Acciones Disponibles:**

**1. Editar Publicación (solo abiertas):**
- Click en botón "✏️ Editar"
- Abre formulario con datos precargados
- Permite modificar todos los campos de la mascota y descripción
- No se pueden editar publicaciones cerradas
- Solo el propietario puede editar

**2. Cerrar Publicación (solo abiertas):**
- Click en botón "✓ Cerrar caso"
- Modal solicita descripción de resolución
- Usuario escribe cómo se resolvió (mínimo 10 caracteres)
- Sistema actualiza BD: `Cerrada=true`, guarda `FechaCierre` y `Resolucion`
- Publicación muestra badge "Cerrada" y texto de resolución

**3. Eliminar Publicación (abiertas y cerradas):**
- Click en botón "🗑️ Eliminar"
- Confirmación con dialog nativo
- Elimina mascota y publicación (cascade delete)
- Acción irreversible

**Layout de Tarjetas:**
```
Publicación ABIERTA:
┌──────────────────────────────────────────────────────────────┐
│ [Ubicación]  [Sexo|Raza]  [Fecha]  [✏️Editar][✓Cerrar][🗑️]  │
└──────────────────────────────────────────────────────────────┘

Publicación CERRADA:
┌──────────────────────────────────────────────────────────────┐
│ [Ubicación]  [Sexo|Raza]  [Fecha]  [✓ Cerrada] [🗑️]         │
│ Resolución: [texto completo]                                 │
│ Cerrado el: [fecha y hora]                                   │
└──────────────────────────────────────────────────────────────┘
```
**Validaciones:**
- Solo el propietario puede ver/editar/eliminar sus publicaciones
- Publicaciones cerradas no se pueden editar (solo eliminar)
- Resolución debe tener mínimo 10 caracteres
- Confirmación obligatoria para eliminar
**Implementación Técnica:**
- **Controllers:** 
  - `AccountController.MisPublicaciones()` (GET)
  - `AccountController.EditarPublicacion()` (GET y POST)
  - `AccountController.CerrarPublicacion()` (POST)
  - `AccountController.EliminarPublicacion()` (POST)
- **Views:** 
  - `MisPublicaciones.cshtml` (lista)
  - `EditarPublicacion.cshtml` (formulario)
- **AJAX:** Llamadas asíncronas para cerrar y eliminar
- **Modal:** Popup con textarea para resolución

## 🎛️ Controladores

### HomeController
**Responsabilidad:** Maneja la página principal y vistas informativas.
**Acciones:**
- `Index()` - GET: Renderiza la página de inicio
- `Privacy()` - GET: (Placeholder) Página de privacidad
**Características:**
- No requiere autenticación
- Acceso público

### MascotasController
**Responsabilidad:** Gestiona todas las operaciones relacionadas con mascotas.
**Inyección de Dependencias:**
```csharp
- ApplicationDbContext _context
- IWebHostEnvironment _environment
- UserManager<Usuario> _userManager
```
**Acciones:**

#### `Buscar()` - GET
```csharp
public async Task<IActionResult> Buscar(
    string? termino, 
    bool sexoMasculino = false, 
    bool sexoFemenino = false, 
    int? raza = null, 
    DateTime? fechaDesde = null)
```
- **Autenticación:** No requerida
- **Funcionalidad:** 
  - Carga todas las mascotas con publicaciones
  - Aplica filtros opcionales
  - Ordena por fecha descendente
  - Pasa datos de filtros a ViewBag
- **Vista:** `Buscar.cshtml`

#### `Publicar()` - GET
```csharp
[Authorize]
public IActionResult Publicar()
```
- **Autenticación:** Requerida
- **Funcionalidad:** Renderiza formulario de publicación
- **Vista:** `Publicar.cshtml`

#### `Publicar()` - POST
```csharp
[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Publicar(Mascota mascota, string? descripcion)
```
- **Autenticación:** Requerida
- **Funcionalidad:**
  - Valida modelo
  - Obtiene usuario actual
  - Crea Mascota y Publicacion
  - Asocia UsuarioId
  - Guarda en BD
  - Redirige con TempData
- **Redirección:** `Buscar` con mensaje de éxito

### AccountController
**Responsabilidad:** Gestiona autenticación, registro y autorización.
**Inyección de Dependencias:**
```csharp
- UserManager<Usuario> _userManager
- SignInManager<Usuario> _signInManager
- ApplicationDbContext _context
```
**Acciones:**

#### `Login()` - GET
```csharp
public IActionResult Login()
```
- Redirige a "/" (usamos popup modal)

#### `Login()` - POST
```csharp
[HttpPost]
[IgnoreAntiforgeryToken]
public async Task<IActionResult> Login(
    string username, 
    string password, 
    bool rememberMe = false)
```
- **Autenticación:** No requerida (es el endpoint de login)
- **Funcionalidad:**
  - Intenta login con SignInManager
  - Retorna OK si exitoso
  - Retorna Unauthorized si falla
  - Retorna 403 si cuenta bloqueada
- **Response:** JSON (para AJAX)

#### `Register()` - GET
```csharp
public IActionResult Register()
```
- Redirige a "/" (usamos popup modal)

#### `Register()` - POST
```csharp
[HttpPost]
[IgnoreAntiforgeryToken]
public async Task<IActionResult> Register(
    string nombreCompleto, 
    string username, 
    string? email, 
    string password, 
    string confirmPassword)
```
- **Funcionalidad:**
  - Valida coincidencia de contraseñas
  - Verifica unicidad de username
  - Verifica unicidad de email (si se proporciona)
  - Crea usuario con UserManager
  - Asigna rol "Usuario"
  - Inicia sesión automáticamente
  - Retorna OK o BadRequest con mensaje
- **Response:** JSON (para AJAX)

#### `Logout()` - POST
```csharp
[HttpPost]
[IgnoreAntiforgeryToken]
public async Task<IActionResult> Logout()
```
- **Funcionalidad:**
  - Cierra sesión con SignInManager
  - Retorna OK
- **Response:** JSON (para AJAX)

#### `MisPublicaciones()` - GET
```csharp
[Authorize]
public async Task<IActionResult> MisPublicaciones()
```
- **Autenticación:** Requerida
- **Funcionalidad:**
  - Obtiene ID del usuario actual
  - Carga sus publicaciones con Include de Mascota
  - Ordena por fecha descendente
  - Retorna vista con lista de publicaciones
- **Vista:** `MisPublicaciones.cshtml`

#### `CerrarPublicacion()` - POST
```csharp
[Authorize]
[HttpPost]
[IgnoreAntiforgeryToken]
public async Task<IActionResult> CerrarPublicacion(int id, string resolucion)
```
- **Autenticación:** Requerida
- **Funcionalidad:**
  - Valida que resolución no esté vacía
  - Busca publicación por ID
  - Verifica que pertenezca al usuario actual
  - Actualiza: Cerrada=true, FechaCierre=now, Resolucion=texto
  - Guarda en BD
  - Retorna OK o BadRequest/NotFound
- **Response:** JSON (para AJAX)

#### `EditarPublicacion()` - GET
```csharp
[Authorize]
[HttpGet]
public async Task<IActionResult> EditarPublicacion(int id)
```
- **Autenticación:** Requerida
- **Funcionalidad:**
  - Obtiene usuario actual
  - Busca publicación con Include de Mascota
  - Verifica propiedad
  - Valida que no esté cerrada
  - Retorna vista con modelo
- **Vista:** `EditarPublicacion.cshtml`

#### `EditarPublicacion()` - POST
```csharp
[Authorize]
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditarPublicacion(int id, Publicacion model)
```
- **Autenticación:** Requerida
- **Funcionalidad:**
  - Valida propiedad y estado (no cerrada)
  - Actualiza todos los campos de Mascota
  - Actualiza descripción de Publicacion
  - Guarda cambios en BD
  - Redirige a MisPublicaciones con mensaje
- **Redirección:** `MisPublicaciones`

#### `EliminarPublicacion()` - POST
```csharp
[Authorize]
[HttpPost]
[IgnoreAntiforgeryToken]
public async Task<IActionResult> EliminarPublicacion(int id)
```
- **Autenticación:** Requerida
- **Funcionalidad:**
  - Verifica propiedad de la publicación
  - Elimina la mascota (cascade elimina publicación)
  - Retorna OK o NotFound
- **Response:** JSON (para AJAX)

#### `AccessDenied()` - GET
```csharp
public IActionResult AccessDenied()
```
- Redirige a "/" cuando se niega acceso

## 🎨 Vistas y UI

### Layout Principal (`_Layout.cshtml`)
**Responsabilidad:** Estructura común de todas las páginas.
**Componentes:**

#### Navbar
```html
- Logo "Encontrá Tu Mascota"
- Links de navegación:
  - Buscar mascotas (público)
  - Publicar mascota (condicional según auth)
- Estado de autenticación:
  - NO AUTH: Botón "Acceder"
  - AUTH: Nombre de usuario clickeable + "Salir"
    - Click en nombre → Mis Publicaciones
    - Hover en nombre → Efecto visual
```

#### Modals (Popups)
**1. Login Modal**
```javascript
- ID: loginPopup
- Funciones: mostrarLogin(), cerrarLogin()
- Formulario: username, password, rememberMe
- Handler: handleLogin() con AJAX
```
**2. Register Modal**
```javascript
- ID: registerPopup
- Funciones: mostrarRegistro(), cerrarRegistro()
- Formulario: nombreCompleto, username, email, password, confirmPassword
- Handler: handleRegister() con AJAX y validación client-side
```
**3. Auth Warning Modal**
```javascript
- ID: authWarningPopup
- Funciones: mostrarAvisoAuth(), cerrarAvisoAuth()
- Botones: "Iniciar Sesión" y "Registrarse"
- Trigger: Click en "Publicar mascota" sin autenticación
```

#### Footer
```html
- Sticky footer
- Copyright y links institucionales
```

#### Scripts Globales
```javascript
// Variables globales
let redirectAfterLogin = null;
// Funciones de UI
- mostrarLogin()
- mostrarLoginConRedireccion(url)
- cerrarLogin()
- mostrarRegistro()
- cerrarRegistro()
- mostrarAvisoAuth()
- cerrarAvisoAuth()
- handleLogin(event)
- handleRegister(event)
- cerrarSesion()
```

### Vista Home (`Index.cshtml`)
**Contenido:**
- Hero section con título y descripción
- Botones de call-to-action:
  - "Buscar Mascotas"
  - "Publicar Mascota Encontrada"
- Secciones informativas:
  - Cómo funciona
  - Estadísticas (placeholder)
  - Testimonios (placeholder)

### Vista Buscar (`Buscar.cshtml`)
**Secciones:**

#### 1. Filtros de Búsqueda
```html
Formulario GET con filtros:
- Ubicación (text input)
- Fecha Desde (date input)
- Sexo (checkboxes: Masculino, Femenino)
- Raza (select dropdown)
- Botones: "Buscar" y "Limpiar"
```

#### 2. Resultados
```html
- Contador: "Mascotas encontradas (N)"
- Grid de tarjetas responsive
- Cada tarjeta contiene:
  - Foto (o placeholder)
  - Ubicación (título)
  - Detalles: Sexo, Raza, Fecha
  - Descripción (si existe)
  - Bloque de contacto (blur condicional)
```

#### 3. Estados Especiales
- Sin resultados de búsqueda
- Base de datos vacía
- Mensaje de éxito post-publicación (TempData)
**JavaScript Interactivo:**
```javascript
// Checkboxes de sexo mutuamente exclusivos
sexoMasculino.addEventListener('change', ...)
sexoFemenino.addEventListener('change', ...)
// Date picker mejorado
fechaInput.addEventListener('click', ...)
```

### Vista Publicar (`Publicar.cshtml`)
**Estructura:**

#### Formulario de Publicación
```html
POST /Mascotas/Publicar
Campos:
1. Foto URL (text, opcional)
2. Ubicación (text, requerido)
3. Sexo (radio buttons)
4. Raza (select)
5. Descripción (textarea, opcional)
6. Nombre Contacto (text, requerido)
7. Teléfono Contacto (text, requerido, validación especial)
8. Email Contacto (email, opcional)
Botones:
- Publicar (submit)
- Cancelar (link a Buscar)
```
**Validaciones Client-Side:**
```html
- asp-validation-for en cada campo
- Validation summary para errores generales
- HTML5 validation attributes
```

### Vista Mis Publicaciones (`MisPublicaciones.cshtml`)
**Estructura:**

#### Header
```html
- Título: "Mis Publicaciones"
- Contador: "Tienes X publicaciones"
```

#### Lista de Publicaciones
```html
- Cards responsivos en grid
- Layout de 3 columnas por card:
  1. Izquierda: Ubicación (texto grande)
  2. Centro: Detalles (Sexo, Raza, Fecha)
  3. Derecha: Acciones (botones según estado)
- Botones para publicaciones ABIERTAS:
  - "✏️ Editar" (azul) - Link a formulario
  - "✓ Cerrar caso" (verde) - Abre modal
  - "🗑️ Eliminar" (rojo) - Confirmación y AJAX
- Botones para publicaciones CERRADAS:
  - "✓ Caso cerrado" (badge gris)
  - "🗑️ Eliminar" (rojo) - Confirmación y AJAX
  - Muestra resolución y fecha de cierre
```

#### Modal de Cierre
```html
- ID: modalCerrar
- Textarea para resolución (min 10 chars)
- Botones: "Confirmar cierre" y "Cancelar"
- Validación: mínimo 10 caracteres
```

#### Estado Vacío
```html
- Mensaje: "No tienes publicaciones todavía"
- Link a "Publicar mascota"
```

#### JavaScript
```javascript
// Funciones globales
- abrirModalCerrar(publicacionId, ubicacion)
- cerrarModalCerrar()
- confirmarCerrar()
- confirmarEliminar(id, ubicacion)
- eliminarPublicacion(id)
// AJAX endpoints
- POST /Account/CerrarPublicacion
- POST /Account/EliminarPublicacion
```

**Estilos Embebidos:**
```css
- .publicaciones-container: Grid responsive
- .publicacion-card: Card con sombra y hover
- .publicacion-layout: Flexbox de 3 columnas
- .btn-editar: Botón azul para editar
- .btn-cerrar-caso: Botón verde para cerrar
- .btn-eliminar: Botón rojo para eliminar
- .modal-cerrar: Overlay con popup centrado
- .publicacion-cerrada: Estilo para casos cerrados
```

### Vista Editar Publicación (`EditarPublicacion.cshtml`)
**Estructura:**

#### Header
```html
- Título: "✏️ Editar publicación"
- Subtítulo: "Modifica los datos de tu publicación"
```

#### Formulario
```html
POST /Account/EditarPublicacion
Campos (precargados con datos actuales):
1. Foto URL (text, **requerido**)
2. Ubicación (text, requerido)
3. Sexo (radio buttons, precargado)
4. Raza (select, precargado)
5. Descripción (textarea, opcional)
6. Nombre Contacto (text, requerido)
7. Teléfono Contacto (text, requerido)
8. Email Contacto (email, **requerido**)
Hidden inputs:
- Publicacion.Id
- Mascota.Id
Botones:
- "💾 Guardar cambios" (naranja)
- "❌ Cancelar" (gris, vuelve a lista)
```

**Validaciones Client-Side:**
```html
- asp-validation-for en cada campo
- Validation summary para errores generales
- HTML5 validation attributes
- AntiForgeryToken incluido
```

**Estilos Embebidos:**
```css
- .editar-container: Contenedor centrado con sombra
- .editar-header: Encabezado con borde inferior
- .form-group: Grupos de formulario espaciados
- .radio-group: Layout para radio buttons
- .form-actions: Botones en flexbox
- .btn-guardar: Botón principal naranja
- .btn-cancelar: Botón secundario gris
- Responsive para móviles
```

## 🔐 Sistema de Autenticación

### ASP.NET Core Identity
**Configuración en Program.cs:**
```csharp
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/";
    options.AccessDeniedPath = "/";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});
```

### Roles del Sistema
**Roles Definidos:**
1. **Admin** - Administradores del sistema
2. **Usuario** - Usuarios registrados estándar
**Creación Automática:**
```csharp
// DbInitializer.cs
string[] roles = { "Admin", "Usuario" };
foreach (var roleName in roles)
{
    if (!await roleManager.RoleExistsAsync(roleName))
    {
        await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}
```

### Usuarios por Defecto
**Cuenta Admin #1:**
- Username: `admin@admin.com`
- Password: `Admin123`
- Rol: Admin
**Cuenta Admin #2:**
- Username: `admin`
- Password: `Admin1`
- Rol: Admin

### Protección de Rutas
**Attribute-Based Authorization:**
```csharp
[Authorize]
public IActionResult Publicar()
```
**Conditional Rendering:**
```razor
@if (User.Identity?.IsAuthenticated == true)
{
    // Contenido para usuarios autenticados
}
else
{
    // Contenido para visitantes
}
```

## 💾 Base de Datos

### Conexión
**String de Conexión:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EncontraTuMascotaDB;Trusted_Connection=true;MultipleActiveResultSets=true"
}
```
**Tipo:** SQL Server LocalDB  
**Nombre:** EncontraTuMascotaDB

### DbContext
**Clase:** `ApplicationDbContext`  
**Hereda de:** `IdentityDbContext<Usuario>`
```csharp
public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public DbSet<Mascota> Mascotas { get; set; }
    public DbSet<Publicacion> Publicaciones { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Relación Mascota -> Publicaciones (1:N)
        builder.Entity<Mascota>()
            .HasMany(m => m.Publicaciones)
            .WithOne(p => p.Mascota)
            .HasForeignKey(p => p.MascotaId)
            .OnDelete(DeleteBehavior.Cascade);
        // Relación Usuario -> Publicaciones (1:N)
        builder.Entity<Usuario>()
            .HasMany(u => u.Publicaciones)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
```

### Tablas

#### Tablas de la Aplicación
**1. Mascotas**
```sql
- Id (int, PK, IDENTITY)
- Sexo (int, enum)
- Raza (int, enum)
- FotoUrl (nvarchar(MAX), nullable)
- Ubicacion (nvarchar(200), NOT NULL)
- FechaPublicacion (datetime2, NOT NULL)
- NombreContacto (nvarchar(100), NOT NULL)
- TelefonoContacto (nvarchar(20), NOT NULL)
- EmailContacto (nvarchar(100), nullable)
```
**2. Publicaciones**
```sql
- Id (int, PK, IDENTITY)
- MascotaId (int, FK -> Mascotas.Id, NOT NULL)
- UsuarioId (nvarchar(450), FK -> AspNetUsers.Id, nullable)
- Descripcion (nvarchar(MAX), nullable)
- Contacto (nvarchar(200), nullable)
- Fecha (datetime2, NOT NULL)
- Cerrada (bit, NOT NULL, default: 0)
- FechaCierre (datetime2, nullable)
- Resolucion (nvarchar(500), nullable)
```

#### Tablas de Identity (8 tablas)
1. **AspNetUsers** - Usuarios del sistema
2. **AspNetRoles** - Roles disponibles
3. **AspNetUserRoles** - Relación usuarios-roles
4. **AspNetUserClaims** - Claims de usuarios
5. **AspNetRoleClaims** - Claims de roles
6. **AspNetUserLogins** - Logins externos
7. **AspNetUserTokens** - Tokens de usuario
8. **__EFMigrationsHistory** - Historial de migraciones

### Migraciones
**Migraciones Aplicadas:**
1. `20251103231832_InitialCreate` - Crea tablas Mascotas y Publicaciones
2. `20251104000638_AddIdentity` - Agrega sistema de Identity completo
3. `20251104233504_AddPublicacionCerrada` - Agrega campos de cierre a Publicaciones (Cerrada, FechaCierre, Resolucion)
**Aplicación Automática:**
```csharp
// Program.cs
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate(); // Aplica migraciones pendientes
    await DbInitializer.Initialize(scope.ServiceProvider); // Seed
}
```

### Datos de Prueba (Seed)
**Clase:** `DatosDePrueba`
**Funcionalidad:**
- Genera 10 mascotas de prueba con datos realistas
- Se ejecuta solo si la BD está vacía
- Datos aleatorios pero coherentes
**Ubicaciones de Ejemplo:**
- "Av. Corrientes 1500, CABA"
- "Parque Centenario, Caballito"
- "Plaza San Martín, Retiro"
- etc.

### Inicializador de Roles y Usuarios
**Clase:** `DbInitializer`
**Ejecuta en Startup:**
1. Crea roles "Admin" y "Usuario" si no existen
2. Crea dos cuentas admin si no existen
3. Asigna rol Admin a ambas cuentas
4. Registra en consola el resultado

## 🔧 Helpers y Utilidades

### 1. TelefonoArgentinoAttribute
**Tipo:** Custom Validation Attribute  
**Ubicación:** `Helpers/TelefonoArgentino.cs`
**Funcionalidad:**  
Valida que un teléfono tenga formato argentino.
**Formatos Aceptados:**
```
- +54 11 1234-5678
- 011 1234-5678
- 11 1234 5678
- 1112345678
- +54 9 11 1234-5678
```
**Implementación:**
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class TelefonoArgentinoAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(...)
    {
        // Regex para validar formato
        // Retorna Success o Error
    }
}
```
**Uso:**
```csharp
[TelefonoArgentino(ErrorMessage = "...")]
public string TelefonoContacto { get; set; }
```

### 2. Messages (Clase Estática)
**Ubicación:** `Helpers/Messages.cs`
**Funcionalidad:**  
Centraliza mensajes de la aplicación para consistencia.
**Constantes:**
```csharp
public static class Messages
{
    public const string RegistroExitoso = "...";
    public const string LoginFallido = "...";
    public const string PublicacionExitosa = "...";
    // etc.
}
```
**Beneficios:**
- Fácil mantenimiento
- Consistencia en mensajes
- Localización futura simplificada

### 3. DatosDePrueba
**Ubicación:** `Helpers/DatosDePrueba.cs`
**Funcionalidad:**  
Genera datos de prueba realistas para desarrollo.
**Método Principal:**
```csharp
public static List<Mascota> ObtenerMascotas()
{
    // Retorna 10 mascotas con datos coherentes
}
```
**Uso:**
```csharp
// Program.cs
if (!context.Mascotas.Any())
{
    var mascotas = DatosDePrueba.ObtenerMascotas();
    context.Mascotas.AddRange(mascotas);
    await context.SaveChangesAsync();
}
```

## ✅ Validaciones

### Validaciones de Modelo (Data Annotations)

#### Mascota
```csharp
[Required(ErrorMessage = "La ubicación es obligatoria")]
[StringLength(200)]
public string Ubicacion { get; set; }
[Required(ErrorMessage = "El nombre de contacto es obligatorio")]
[StringLength(100)]
public string NombreContacto { get; set; }
[Required(ErrorMessage = "El teléfono es obligatorio")]
[TelefonoArgentino(ErrorMessage = "Formato de teléfono argentino inválido")]
public string TelefonoContacto { get; set; }
[EmailAddress(ErrorMessage = "Formato de email inválido")]
public string? EmailContacto { get; set; }
```

#### Usuario (Identity)
```csharp
[StringLength(100)]
public string? NombreCompleto { get; set; }
// Email y UserName validados por Identity
```

### Validaciones de Controlador

#### AccountController - Register
```csharp
// Contraseñas coinciden
if (password != confirmPassword)
    return BadRequest("Las contraseñas no coinciden");
// Username único
var existingUser = await _userManager.FindByNameAsync(username);
if (existingUser != null)
    return BadRequest("El nombre de usuario ya está en uso");
// Email único (si se proporciona)
if (!string.IsNullOrWhiteSpace(email))
{
    var existingEmail = await _userManager.FindByEmailAsync(email);
    if (existingEmail != null)
        return BadRequest("El email ya está registrado");
}
```

#### MascotasController - Publicar
```csharp
if (!ModelState.IsValid)
    return View(mascota);
```

### Validaciones Client-Side

#### JavaScript (Registro)
```javascript
// Validación de contraseña
if (password.length < 5) {
    alert('La contraseña debe tener al menos 5 caracteres');
    return;
}
if (!/[a-z]/.test(password)) {
    alert('La contraseña debe contener al menos una minúscula');
    return;
}
if (!/\d/.test(password)) {
    alert('La contraseña debe contener al menos un número');
    return;
}
if (password !== confirmPassword) {
    alert('Las contraseñas no coinciden');
    return;
}
```

#### HTML5 Validation
```html
<input type="text" required minlength="3" maxlength="100" />
<input type="email" />
<input type="date" />
```

## 📱 Características de UI/UX

### Diseño Responsive
- ✅ Mobile-first approach
- ✅ Breakpoints para tablet y desktop
- ✅ Grid system flexible
- ✅ Cards adaptables

### Material Design
- ✅ Colores corporativos (#FF6B35)
- ✅ Sombras y elevaciones
- ✅ Transiciones suaves
- ✅ Iconografía Unicode

### Accesibilidad
- ✅ Labels asociados a inputs
- ✅ ARIA attributes en modals
- ✅ Contraste de colores adecuado
- ✅ Navegación por teclado

### Performance
- ✅ AJAX para formularios (sin recargas)
- ✅ Lazy loading implícito en Entity Framework
- ✅ Queries asíncronas
- ✅ CSS minificado en producción

## 🚀 Flujo de Usuario Completo

### Usuario No Autenticado
1. **Landing** → Home (`/`)
2. **Explorar** → Buscar Mascotas (`/Mascotas/Buscar`)
   - Ve todas las mascotas
   - Aplica filtros
   - Ve contactos con blur
   - Click en "Iniciar sesión" → Modal de login
3. **Registrarse** → Click "Registrarse" → Modal
   - Completa formulario
   - Sistema valida
   - Crea cuenta y hace auto-login
4. **Intentar Publicar** → Click "Publicar mascota"
   - Sistema detecta falta de auth
   - Muestra popup de advertencia
   - Redirige a login

### Usuario Autenticado
1. **Login** → Modal o redirect post-registro
2. **Explorar (Full Access)** → Buscar Mascotas
   - Ve todas las mascotas
   - Ve contactos SIN blur
   - Puede contactar directamente
3. **Publicar** → Click "Publicar mascota"
   - Formulario accesible directamente
   - Completa datos
   - Sistema asocia publicación a usuario
   - Redirige con mensaje de éxito
4. **Panel Personal** → Click en nombre de usuario
   - Accede a `/Account/MisPublicaciones`
   - Ve lista de sus publicaciones
   - Información organizada en 3 columnas
5. **Gestionar Publicación** → Desde panel
   - Publicaciones abiertas: botón "Cerrar caso"
   - Click en botón → Modal con textarea
   - Escribe resolución (min 10 chars)
   - Confirma cierre
   - Sistema actualiza estado y guarda resolución
6. **Ver Resoluciones** → En panel
   - Publicaciones cerradas muestran:
     - Badge "Cerrada"
     - Fecha de cierre
     - Texto de resolución
7. **Logout** → Click "Salir"
   - Sistema cierra sesión
   - Vuelve a estado no autenticado

## 📈 Mejoras Futuras (Roadmap)

### Funcionalidades Potenciales
1. ✅ **Panel de Usuario Avanzado** - COMPLETADO 100%
   - ✅ Dashboard personal con publicaciones
   - ✅ Gestión de publicaciones (editar, cerrar, eliminar)
   - ✅ Validaciones de seguridad y propiedad
   - ✅ Estados diferenciados (abierta/cerrada)
   - ✅ Formulario de edición completo
   - ✅ Confirmaciones para acciones destructivas
   - 🔄 Futuro: Historial de búsquedas
   - 🔄 Futuro: Estadísticas personales
   - 🔄 Futuro: Reabrir casos cerrados
2. **Mensajería Interna**
   - Chat entre usuarios
   - Notificaciones
   - Sistema de matches
3. **Geolocalización**
   - Mapa interactivo
   - Filtro por proximidad
   - Ubicación GPS de mascotas
4. **Carga de Imágenes**
   - Upload de fotos (no solo URL)
   - Múltiples fotos por mascota
   - Compresión automática
5. **Sistema de Reportes**
   - Reportar publicaciones inadecuadas
   - Moderación por admins
   - Ban temporal de usuarios
6. **Estadísticas**
   - Dashboard de admin
   - Métricas de reencuentros
   - Análisis de datos
7. **API REST**
   - Endpoints públicos
   - Autenticación JWT
   - Documentación Swagger
8. **Notificaciones**
   - Alertas de nuevas publicaciones
   - Matches automáticos por descripción
   - Emails transaccionales

## 🔍 Troubleshooting Común

### Problemas de Autenticación
**Síntoma:** No puedo iniciar sesión  
**Solución:**
- Verificar que username sea correcto (no email)
- Probar con cuentas admin: `admin`/`Admin1`
- Revisar si cuenta está bloqueada (5 intentos fallidos)
**Síntoma:** Datos de contacto siguen con blur  
**Solución:**
- Verificar que sesión esté iniciada (ver saludo en navbar)
- Recargar página después de login
- Limpiar cookies si hay problemas persistentes
**Síntoma:** No veo mis publicaciones en el panel  
**Solución:**
- Verificar que hayas publicado mascotas previamente
- Recargar la página
- Revisar en BD que UsuarioId esté asignado correctamente

### Problemas de Base de Datos
**Síntoma:** Error de migración  
**Solución:**
```powershell
cd EncontraTuMascota
dotnet ef database drop --context ApplicationDbContext
dotnet ef database update
```
**Síntoma:** No hay datos de prueba  
**Solución:**
- Verificar que BD esté vacía al iniciar app
- Revisar consola para mensajes de seed
- Ejecutar `DatosDePrueba.ObtenerMascotas()` manualmente
**Síntoma:** Publicaciones sin usuario asignado  
**Solución:**
- Ejecutar SQL: `UPDATE Publicaciones SET UsuarioId = (SELECT Id FROM AspNetUsers WHERE UserName = 'admin') WHERE UsuarioId IS NULL`
- Usar archivo `SQLs.sql` para verificar y corregir

### Problemas de Validación
**Síntoma:** Formulario no valida teléfono  
**Solución:**
- Usar formato argentino válido
- Ejemplos: `1112345678`, `+54 11 1234-5678`
- Revisar regex en `TelefonoArgentinoAttribute`
**Síntoma:** No puedo cerrar una publicación  
**Solución:**
- Verificar que el texto de resolución tenga mínimo 10 caracteres
- Asegurarse de ser el propietario de la publicación
- Revisar consola del navegador para errores AJAX

## 📞 Cuentas de Prueba

### Administradores
| Username | Password | Rol | Email |
|----------|----------|-----|-------|
| `admin` | `Admin1` | Admin | admin@sistema.com |
| `admin@admin.com` | `Admin123` | Admin | admin@admin.com |

**Nota:** El usuario `admin` tiene 10 publicaciones de prueba asignadas automáticamente.

### Usuarios Regulares
Crear mediante formulario de registro en la aplicación.
**Requisitos de Contraseña:**
- Mínimo 5 caracteres
- Al menos una minúscula
- Al menos un dígito
- Mayúscula opcional

## 🗂️ Archivo SQLs.sql

### Descripción
Archivo ubicado en la raíz del proyecto con consultas SQL útiles para administración y debugging.

### Consultas Incluidas
1. **Listar todos los usuarios** - Con información completa y credenciales en comentarios
2. **Usuarios con sus roles** - JOIN con AspNetRoles
3. **Información de cuentas admin** - Credenciales de administradores
4. **Verificar y asignar publicaciones** - Asignar publicaciones huérfanas a admin
5. **Ver todas las publicaciones** - Con detalles de mascota y usuario
6. **Publicaciones por usuario** - Estadísticas agrupadas
7. **Mascotas publicadas** - Lista completa con enums legibles
8. **Roles del sistema** - Cantidad de usuarios por rol
9. **Últimas publicaciones** - TOP 10 ordenadas por fecha
10. **Estadísticas generales** - Contadores globales
11. **Buscar usuario por nombre** - Con LIKE pattern
12. **Eliminar publicaciones de usuario** - Template comentado
13. **Cerrar todas las publicaciones** - Para testing
14. **Reabrir todas las publicaciones** - Para testing
15. **Información de credenciales** - Comentario con passwords

### Uso Recomendado
```powershell
# Conectarse a la BD
sqlcmd -S "(localdb)\mssqllocaldb" -d EncontraTuMascotaDB
# Ejecutar consultas del archivo según necesidad
```

## 📄 Licencia y Créditos
**Proyecto:** Encontrá Tu Mascota  
**Framework:** ASP.NET Core MVC 8.0  
**Institución:** Universidad ORT Uruguay  
**Materia:** Programación .NET  
**Fecha:** Noviembre 2025