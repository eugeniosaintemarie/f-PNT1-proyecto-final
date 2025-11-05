# Punto 2 · Definición de la aplicación "Encontrá Tu Mascota"
Este documento sintetiza lo solicitado en el punto 2 y sus sub-items para la aplicación web acordada con el docente.

## a. Objetivo de la aplicación
Encontrá Tu Mascota es una plataforma web colaborativa que permite:
- Publicar mascotas encontradas en la vía pública junto con datos de contacto del rescatista.
- Navegar y filtrar publicaciones existentes para que los dueños identifiquen rápidamente a su mascota perdida.
- Proteger los datos de contacto de las publicaciones, mostrándolos completos solo a usuarios autenticados.

El propósito final es acelerar el reencuentro entre mascotas perdidas y sus familias mediante un sistema simple y centralizado.

## b. Funcionalidades de la aplicación
| Código | Funcionalidad | Descripción breve |
|--------|---------------|-------------------|
| F1 | **Visualización pública de mascotas** | Invitados (no autenticados) consultan la grilla de publicaciones, cantidad total y descripción de cada mascota sin ver datos de contacto. |
| F2 | **Registro de usuarios** | Permite crear cuentas para acceder a datos de contacto, verificando unicidad de usuario, formato de email y fortaleza de contraseña. |
| F3 | **Gestión autenticada de publicaciones** | Usuarios registrados ven datos de contacto completos y pueden crear, editar o retirar publicaciones propias de mascotas encontradas. |

## c. Priorización de funcionalidades
| Funcionalidad | Prioridad | Motivo |
|---------------|-----------|--------|
| F1 Visualización pública de mascotas | **Alta (P1)** | Genera el primer contacto con el servicio y demuestra valor incluso a visitantes anónimos. |
| F2 Registro de usuarios | **Alta (P1)** | Habilita el acceso controlado a datos sensibles y habilita el resto del flujo autenticado. |
| F3 Gestión autenticada de publicaciones | **Alta (P1)** | Concreta el objetivo del negocio: que usuarios registrados contacten y publiquen para reunir mascotas con sus dueños. |

## d. Pasos principales por funcionalidad
### F1 · Visualización pública de mascotas
1. Invitado abre la página principal o la vista "Buscar Mascotas".
2. El sistema carga la cantidad total de mascotas publicadas y las renderiza en formato de tarjetas.
3. Cada tarjeta muestra información descriptiva (foto, fecha, ubicación, sexo, raza, descripción) sin exponer datos de contacto.
4. Se destaca un llamado a la acción para registrarse o iniciar sesión si desea contactar o publicar.
### F2 · Registro de usuarios
1. Invitado selecciona "Registrarse" desde la navegación principal.
2. Completa formulario con nombre de usuario, email, contraseña y confirmación de contraseña.
3. El backend valida unicidad del nombre de usuario/email, formato de email y políticas de contraseña.
4. Si hay errores, se muestran mensajes de validación en tiempo real o tras enviar el formulario.
5. Al superar las validaciones, se crea el perfil, se inicia sesión automáticamente opcionalmente y se redirige al área autenticada.
### F3 · Gestión autenticada de publicaciones
1. Usuario registrado inicia sesión y accede a la vista de publicaciones.
2. El sistema habilita la visualización completa de datos de contacto en cada tarjeta.
3. El usuario navega al formulario "Publicar Mascota" para crear un nuevo post.
4. Completa datos de la publicación (foto, ubicación, especie/raza, descripción, medios de contacto) y envía.
5. El servidor valida campos requeridos, formato de teléfono/email y pertenencia de la publicación al usuario.
6. Tras confirmación, la publicación queda visible para todos (contacto visible solo para autenticados) y el usuario puede editarla o retirarla desde su tablero.

## e. Datos manejados y validaciones
| Funcionalidad | Datos ingresados/mostrados | Validaciones principales |
|---------------|---------------------------|--------------------------|
| F1 Visualización pública | Datos descriptivos de cada mascota (foto, ubicación, sexo, raza, descripción, fecha) sin datos de contacto visibles | Garantía de anonimato del contacto en modo invitado; control de caché para no exponer datos sensibles; paginación/contador consistente con fuente de datos. |
| F2 Registro de usuarios | Nombre de usuario, Email, Contraseña, Confirmación | Validación de unicidad (DB o repositorio), regex para email válido, políticas de contraseña (longitud mínima, complejidad, coincidencia con confirmación), manejo de errores amigables. |
| F3 Gestión autenticada | Datos completos de publicación (incluye contacto), datos de sesión del usuario, histórico de publicaciones propias | Data Annotations para campos obligatorios, validación custom de teléfono argentino, protección contra edición de publicaciones ajenas, autorización para mostrar contacto solo a usuarios autenticados, registros de auditoría básicos. |

## f. Diagrama de clases (negocio)
Se utiliza el diagrama `UML.uxf` (actualizado 29/10/2025) que incluye:
- Clases de negocio: `Mascota`, `Publicacion`, `Usuario`
- Enums: `Sexo`, `Raza`
- Relaciones 1:N entre Mascota y Publicacion
- Helpers relevantes (`DatosDePrueba`, `TelefonoArgentinoAttribute`, `Messages`)

Para el documento Word adjuntar captura o exportación del archivo `UML.uxf` desde UMLet.

## g. Esquema de páginas y navegación
| Página | URL | Contenido principal | Navegación |
|--------|-----|---------------------|------------|
| Home | `/Home/Index` | Presenta objetivo del sitio, accesos rápidos a publicar y buscar, footer informativo. | Navbar con links a Publicar, Buscar, Acceder/Salir. |
| Publicar Mascota | `/Mascotas/Publicar` | Formulario de carga con campos y validaciones; feedback de éxito tras publicación. | Botón "Buscar Mascotas" en navbar; tras publicar redirige a Buscar con mensaje. |
| Buscar Mascotas | `/Mascotas/Buscar` | Filtros avanzados (ubicación, sexo, raza, fecha); grilla de cards con datos y contacto blur/no blur; modal de login. | Navbar, enlaces a Publicar; login modal desde botón Acceder. |
| Login modal | (overlay en `_Layout`) | Formulario emergente para autenticación. | Acceso desde botón "Acceder"; cierre por botón ✕, backdrop o botón Cancelar. |
| (Futuro) Contacto/Privacidad | `/Home/Privacy` (placeholder) | Contenido estático previsto para cumplimiento legal. | Acceso desde footer o menú según se defina. |

El flujo principal parte de Home → (Publicar | Buscar). El estado de autenticación modifica botones y datos visibles, manteniendo navegación consistente.