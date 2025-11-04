using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Data;

/// <summary>
/// Este es el contexto de la base de datos.
/// Acá definimos todas las tablas que va a tener nuestra BD.
/// Entity Framework se encarga de crear las tablas, relaciones y todo eso.
/// Ahora hereda de IdentityDbContext para incluir las tablas de usuarios, roles, etc.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Cada DbSet es una tabla en la base de datos
    public DbSet<Mascota> Mascotas { get; set; } = null!;
    public DbSet<Publicacion> Publicaciones { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuramos la relación entre Mascota y Publicacion
        // Una mascota puede tener muchas publicaciones
        modelBuilder.Entity<Mascota>()
            .HasMany(m => m.Publicaciones)
            .WithOne(p => p.Mascota)
            .HasForeignKey(p => p.MascotaId)
            .OnDelete(DeleteBehavior.Cascade); // Si borrás una mascota, se borran sus publicaciones

        // Configuramos la relación entre Usuario y Publicacion
        // Un usuario puede tener muchas publicaciones
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Publicaciones)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull); // Si borrás un usuario, las publicaciones quedan pero sin usuario

        // Configuramos índices para mejorar la performance de las búsquedas
        modelBuilder.Entity<Mascota>()
            .HasIndex(m => m.Ubicacion);

        modelBuilder.Entity<Mascota>()
            .HasIndex(m => m.FechaPublicacion);

        modelBuilder.Entity<Mascota>()
            .HasIndex(m => new { m.Sexo, m.Raza }); // Índice compuesto para búsquedas por sexo y raza

        modelBuilder.Entity<Publicacion>()
            .HasIndex(p => p.Fecha);
    }
}
