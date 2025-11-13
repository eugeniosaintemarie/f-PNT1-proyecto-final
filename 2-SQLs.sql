-- ========================================
-- SQLs Útiles para EncontraTuMascota
-- Base de datos: EncontraTuMascotaDB
-- ========================================

-- ========================================
-- 1. CONSULTAR TODOS LOS USUARIOS
-- ========================================
-- Muestra todos los usuarios con su información básica
-- NOTA: Las contraseñas están hasheadas, no se pueden ver en texto plano
SELECT 
    Id,
    UserName AS 'Usuario',
    Email,
    NombreCompleto AS 'Nombre Completo',
    FechaRegistro AS 'Fecha de Registro',
    EmailConfirmed AS 'Email Confirmado',
    PhoneNumber AS 'Teléfono',
    LockoutEnabled AS 'Bloqueo Habilitado',
    AccessFailedCount AS 'Intentos Fallidos'
FROM AspNetUsers
ORDER BY FechaRegistro DESC;

-- ========================================
-- 2. USUARIOS CON SUS ROLES
-- ========================================
SELECT 
    u.UserName AS 'Usuario',
    u.Email,
    u.NombreCompleto AS 'Nombre Completo',
    r.Name AS 'Rol',
    u.FechaRegistro AS 'Fecha Registro'
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY r.Name, u.UserName;

-- ========================================
-- 3. INFORMACIÓN DE CUENTAS ADMIN
-- ========================================
-- Las contraseñas son:
-- admin@admin.com -> Admin123
-- admin -> Admin1
SELECT 
    UserName AS 'Usuario',
    Email,
    'Ver comentario arriba' AS 'Contraseña'
FROM AspNetUsers
WHERE UserName IN ('admin', 'admin@admin.com');

-- ========================================
-- 4. VERIFICAR Y ASIGNAR PUBLICACIONES AL USUARIO ADMIN
-- ========================================
-- Obtener el ID del usuario admin
DECLARE @AdminId NVARCHAR(450);
SELECT @AdminId = Id FROM AspNetUsers WHERE UserName = 'admin';

-- Mostrar el ID encontrado
SELECT @AdminId AS 'ID del usuario admin';

-- Ver cuántas publicaciones hay en total
SELECT COUNT(*) AS 'Total publicaciones en BD' FROM Publicaciones;

-- Ver publicaciones del usuario admin
SELECT 
    p.Id,
    p.MascotaId,
    p.UsuarioId,
    p.Fecha,
    p.Cerrada,
    u.UserName AS 'Usuario'
FROM Publicaciones p
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
WHERE p.UsuarioId = @AdminId;

-- Actualizar todas las publicaciones sin usuario asignado (si las hay)
UPDATE Publicaciones 
SET UsuarioId = @AdminId
WHERE UsuarioId IS NULL;

-- Verificar cuántas se actualizaron
SELECT COUNT(*) AS 'Publicaciones asignadas a admin'
FROM Publicaciones
WHERE UsuarioId = @AdminId;

-- ========================================
-- 5. VER TODAS LAS PUBLICACIONES
-- ========================================
SELECT 
    p.Id,
    m.Ubicacion AS 'Ubicación',
    m.Sexo,
    m.Raza,
    p.Fecha AS 'Fecha Publicación',
    u.UserName AS 'Usuario',
    p.Cerrada AS 'Cerrada',
    p.FechaCierre AS 'Fecha Cierre',
    p.Resolucion
FROM Publicaciones p
INNER JOIN Mascotas m ON p.MascotaId = m.Id
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
ORDER BY p.Fecha DESC;

-- ========================================
-- 6. PUBLICACIONES POR USUARIO
-- ========================================
SELECT 
    u.UserName AS 'Usuario',
    COUNT(p.Id) AS 'Total Publicaciones',
    SUM(CASE WHEN p.Cerrada = 1 THEN 1 ELSE 0 END) AS 'Cerradas',
    SUM(CASE WHEN p.Cerrada = 0 THEN 1 ELSE 0 END) AS 'Abiertas'
FROM AspNetUsers u
LEFT JOIN Publicaciones p ON u.Id = p.UsuarioId
GROUP BY u.UserName
ORDER BY COUNT(p.Id) DESC;

-- ========================================
-- 7. MASCOTAS PUBLICADAS (TODAS)
-- ========================================
SELECT 
    m.Id,
    m.Ubicacion,
    CASE m.Sexo 
        WHEN 0 THEN 'Masculino'
        WHEN 1 THEN 'Femenino'
    END AS 'Sexo',
    CASE m.Raza
        WHEN 0 THEN 'Labrador'
        WHEN 1 THEN 'Golden Retriever'
        WHEN 2 THEN 'Pastor Alemán'
        WHEN 3 THEN 'Bulldog'
        WHEN 4 THEN 'Beagle'
        WHEN 5 THEN 'Poodle'
        WHEN 6 THEN 'Yorkshire Terrier'
        WHEN 7 THEN 'Chihuahua'
        WHEN 8 THEN 'Husky Siberiano'
        WHEN 9 THEN 'Cocker Spaniel'
    END AS 'Raza',
    m.FechaPublicacion AS 'Fecha',
    m.NombreContacto AS 'Contacto',
    m.TelefonoContacto AS 'Teléfono'
FROM Mascotas m
ORDER BY m.FechaPublicacion DESC;

-- ========================================
-- 8. ROLES DEL SISTEMA
-- ========================================
SELECT 
    r.Name AS 'Rol',
    COUNT(ur.UserId) AS 'Cantidad de Usuarios'
FROM AspNetRoles r
LEFT JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
GROUP BY r.Name;

-- ========================================
-- 9. ÚLTIMAS PUBLICACIONES (TOP 10)
-- ========================================
SELECT TOP 10
    m.Ubicacion,
    CASE m.Sexo 
        WHEN 0 THEN 'Masculino'
        WHEN 1 THEN 'Femenino'
    END AS 'Sexo',
    p.Fecha AS 'Publicado',
    u.UserName AS 'Por Usuario',
    CASE WHEN p.Cerrada = 1 THEN 'Cerrada' ELSE 'Abierta' END AS 'Estado'
FROM Publicaciones p
INNER JOIN Mascotas m ON p.MascotaId = m.Id
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
ORDER BY p.Fecha DESC;

-- ========================================
-- 10. ESTADÍSTICAS GENERALES
-- ========================================
SELECT 
    (SELECT COUNT(*) FROM AspNetUsers) AS 'Total Usuarios',
    (SELECT COUNT(*) FROM Mascotas) AS 'Total Mascotas',
    (SELECT COUNT(*) FROM Publicaciones) AS 'Total Publicaciones',
    (SELECT COUNT(*) FROM Publicaciones WHERE Cerrada = 1) AS 'Casos Cerrados',
    (SELECT COUNT(*) FROM Publicaciones WHERE Cerrada = 0) AS 'Casos Abiertos';

-- ========================================
-- 11. BUSCAR USUARIO POR NOMBRE
-- ========================================
-- Reemplaza 'admin' por el usuario que quieras buscar
SELECT 
    u.UserName AS 'Usuario',
    u.Email,
    u.NombreCompleto AS 'Nombre Completo',
    r.Name AS 'Rol',
    u.FechaRegistro AS 'Registrado',
    (SELECT COUNT(*) FROM Publicaciones WHERE UsuarioId = u.Id) AS 'Publicaciones'
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName LIKE '%admin%';

-- ========================================
-- 12. CREAR NUEVO USUARIO ADMIN (MANUAL)
-- ========================================
-- NOTA: NO USES ESTO - El sistema ya crea usuarios automáticamente
-- con contraseñas hasheadas. Este es solo para referencia.
-- Para crear usuarios, usa el formulario de registro en la aplicación.

-- ========================================
-- 13. ELIMINAR TODAS LAS PUBLICACIONES DE UN USUARIO
-- ========================================
-- ¡CUIDADO! Esto elimina todas las publicaciones de un usuario
-- Reemplaza 'username' por el usuario real
/*
DELETE FROM Publicaciones 
WHERE UsuarioId = (SELECT Id FROM AspNetUsers WHERE UserName = 'username');
*/

-- ========================================
-- 14. CERRAR TODAS LAS PUBLICACIONES ABIERTAS
-- ========================================
-- Para testing: cierra todas las publicaciones abiertas
/*
UPDATE Publicaciones 
SET 
    Cerrada = 1,
    FechaCierre = GETDATE(),
    Resolucion = 'Caso cerrado automáticamente para pruebas'
WHERE Cerrada = 0;
*/

-- ========================================
-- 15. REABRIR TODAS LAS PUBLICACIONES
-- ========================================
-- Para testing: reabre todas las publicaciones
/*
UPDATE Publicaciones 
SET 
    Cerrada = 0,
    FechaCierre = NULL,
    Resolucion = NULL;
*/

-- ========================================
-- INFORMACIÓN IMPORTANTE
-- ========================================
/*
CREDENCIALES DE ADMIN POR DEFECTO:

Usuario 1:
- Username: admin@admin.com
- Password: Admin123

Usuario 2:
- Username: admin
- Password: Admin1

NOTA: Las contraseñas están hasheadas en la base de datos usando
el algoritmo de ASP.NET Core Identity. No se pueden recuperar en
texto plano. Si olvidas una contraseña, debes:
1. Usar una de las cuentas admin predefinidas arriba
2. Crear una nueva cuenta
3. O resetear la base de datos

Para resetear la base de datos:
1. Detener la aplicación
2. En terminal: dotnet ef database drop
3. En terminal: dotnet ef database update
4. Reiniciar la aplicación
*/
