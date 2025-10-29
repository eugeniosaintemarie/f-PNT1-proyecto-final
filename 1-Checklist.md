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

- [x] **Inyección de dependencias**

  - ✅ Configurar servicios en Program.cs
  - Pendiente: Inyectar DbContext en controladores

- [ ] **Scaffolding**

  - Pendiente: Generar vistas CRUD con scaffolding

- [x] **LINQ básico**

  - ✅ Usado en búsqueda de mascotas (Where, ToList)
  - ✅ Expandir uso con consultas más complejas

- [ ] **Migraciones**

  - Pendiente: Crear migración inicial
  - Pendiente: Aplicar migraciones a BD

- [x] **Inicializador BD (Seed)**

  - ✅ Crear clase de inicialización
  - ✅ Poblar datos de prueba

- [ ] **Identity Management**

  - Pendiente: Instalar Microsoft.AspNetCore.Identity
  - Pendiente: Configurar Identity

- [x] **ViewModels Intro**

  - ✅ Crear ViewModels para vistas complejas
  - ✅ Separar lógica de presentación

- [x] **Autenticación**

  - ✅ Implementar login/registro
  - ✅ Configurar autenticación de usuarios

- [ ] **Roles/Autorización**

  - Pendiente: Definir roles (Admin, Usuario)
  - Pendiente: Implementar [Authorize] en controladores

- [x] **Uso de identidad y adecuación**

  - ✅ Asociar publicaciones con usuarios
  - ✅ Restringir acciones según usuario autenticado

- [x] **MVC**
  - ✅ Patrón MVC implementado
  - ✅ Separación Model-View-Controller

---

## Notas de Progreso

### Completado hasta ahora:

- Estructura MVC básica
- Modelos con validaciones (Mascota, Publicacion, Usuario)
- Controladores funcionales (Home, Mascotas, Contacto, Account)
- Vistas con formularios y búsqueda avanzada con filtros
- Helpers y configuraciones (Messages, GlobalAliases, DatosDePrueba)
- CSS responsive con sticky footer y Material Design
- Navegación completa entre páginas
- Sistema de autenticación con sesiones (login/logout)
- Datos de prueba (10 mascotas)
- Inyección de dependencias (Session, Singleton)
- LINQ con múltiples filtros (ubicación, sexo, raza, fecha)

### Próximos pasos prioritarios:

1. Implementar Entity Framework Core
2. Crear DbContext y configurar conexión a BD
3. Crear migraciones
4. Implementar Identity para autenticación
5. Agregar roles y autorización
6. Implementar herencia en modelos

---

**Última actualización:** 28 de octubre de 2025
