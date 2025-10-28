# Checklist del Proyecto - Encontrá Tu Mascota

## Requisitos Obligatorios del Proyecto

### ✅ Implementados

- [ ] **Herencia**

  - Pendiente: Implementar clases base y herencia en modelos

- [x] **Relaciones y restricciones básicas**

  - ✅ Relación Mascota-Publicación (1:N)
  - ✅ Foreign Key MascotaId en Publicación
  - ✅ Navigation properties implementadas

- [x] **Vistas y controladores**

  - ✅ HomeController con vista Index
  - ✅ MascotasController con vistas Publicar y Buscar
  - ✅ ContactoController con vista Index
  - ✅ Layout compartido (\_Layout.cshtml)

- [x] **Routing básico**

  - ✅ Routing por convención configurado
  - ✅ Rutas: {controller=Home}/{action=Index}/{id?}

- [x] **Model binding**

  - ✅ Formularios vinculados a modelos (Mascota, Usuario, Publicacion)
  - ✅ Validaciones con Data Annotations

- [x] **Paquetes**

  - ✅ Microsoft.VisualStudio.Azure.Containers.Tools.Targets

- [ ] **Persistencia (EF Core)**

  - Pendiente: Implementar DbContext
  - Pendiente: Configurar Entity Framework Core
  - Pendiente: Reemplazar listas en memoria por BD

- [ ] **Inyección de dependencias**

  - Pendiente: Configurar servicios en Program.cs
  - Pendiente: Inyectar DbContext en controladores

- [ ] **Scaffolding**

  - Pendiente: Generar vistas CRUD con scaffolding

- [ ] **LINQ básico**

  - ⚠️ Parcial: Usado en búsqueda de mascotas (Where, ToList)
  - Pendiente: Expandir uso con consultas más complejas

- [ ] **Migraciones**

  - Pendiente: Crear migración inicial
  - Pendiente: Aplicar migraciones a BD

- [ ] **Inicializador BD (Seed)**

  - Pendiente: Crear clase de inicialización
  - Pendiente: Poblar datos de prueba

- [ ] **Identity Management**

  - Pendiente: Instalar Microsoft.AspNetCore.Identity
  - Pendiente: Configurar Identity

- [ ] **ViewModels Intro**

  - Pendiente: Crear ViewModels para vistas complejas
  - Pendiente: Separar lógica de presentación

- [ ] **Autenticación**

  - Pendiente: Implementar login/registro
  - Pendiente: Configurar autenticación de usuarios

- [ ] **Roles/Autorización**

  - Pendiente: Definir roles (Admin, Usuario)
  - Pendiente: Implementar [Authorize] en controladores

- [ ] **Uso de identidad y adecuación**

  - Pendiente: Asociar publicaciones con usuarios
  - Pendiente: Restringir acciones según usuario autenticado

- [x] **MVC**
  - ✅ Patrón MVC implementado
  - ✅ Separación Model-View-Controller

---

## Notas de Progreso

### Completado hasta ahora:

- Estructura MVC básica
- Modelos con validaciones (Mascota, Publicacion, Usuario)
- Controladores funcionales
- Vistas con formularios y búsqueda
- Helpers y configuraciones (Messages, AppConfig, DateTimeHelper)
- CSS responsive con sticky footer
- Navegación completa entre páginas

### Próximos pasos prioritarios:

1. Implementar Entity Framework Core
2. Crear DbContext y configurar conexión a BD
3. Crear migraciones
4. Implementar Identity para autenticación
5. Agregar roles y autorización
6. Crear ViewModels para vistas complejas
7. Implementar herencia en modelos

---

**Última actualización:** 27 de octubre de 2025
