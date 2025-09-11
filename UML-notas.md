# Entidades y clases principales

1. Mascota (entidad fuerte)
- Id (int, PK)
- FotoUrl (string)
- CalleEncontrada (string)
- FechaPublicacion (DateTime)

2. Publicación
- Id (int, PK)
- MascotaId (int, FK → Mascota)
- Descripcion (string)
- Contacto (string)
- Fecha (DateTime)

3. Usuario (entidad débbil)
- Id (int, PK)
- Nombre (string)
- Email (string)