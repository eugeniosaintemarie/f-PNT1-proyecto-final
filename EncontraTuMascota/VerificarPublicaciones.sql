-- Verificar usuarios
SELECT Id, UserName, Email FROM AspNetUsers;

-- Verificar mascotas
SELECT COUNT(*) AS 'Total Mascotas' FROM Mascotas;

-- Verificar publicaciones
SELECT COUNT(*) AS 'Total Publicaciones' FROM Publicaciones;

-- Ver todas las publicaciones con detalles
SELECT 
    p.Id,
    p.MascotaId,
    p.UsuarioId,
    u.UserName AS 'Usuario',
    m.Ubicacion,
    m.Sexo,
    m.Raza,
    p.Fecha,
    p.Cerrada
FROM Publicaciones p
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
LEFT JOIN Mascotas m ON p.MascotaId = m.Id;

-- Asignar publicaciones sin dueño al usuario admin
DECLARE @AdminId NVARCHAR(450);
SELECT @AdminId = Id FROM AspNetUsers WHERE UserName = 'admin';

UPDATE Publicaciones 
SET UsuarioId = @AdminId
WHERE UsuarioId IS NULL;

-- Verificar resultado
SELECT COUNT(*) AS 'Publicaciones del admin' 
FROM Publicaciones 
WHERE UsuarioId = (SELECT Id FROM AspNetUsers WHERE UserName = 'admin');
