using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using EncontraTuMascota.Models;

namespace EncontraTuMascota.Data;

public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Mascota> Mascotas { get; set; } = null!;
    public DbSet<Publicacion> Publicaciones { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Mascota>()
            .HasMany(m => m.Publicaciones)
            .WithOne(p => p.Mascota)
            .HasForeignKey(p => p.MascotaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Publicaciones)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Mascota>()
            .HasIndex(m => m.Ubicacion);

        modelBuilder.Entity<Mascota>()
            .HasIndex(m => m.FechaPublicacion);

        modelBuilder.Entity<Mascota>()
            .HasIndex(m => new { m.Sexo, m.Raza });

        modelBuilder.Entity<Publicacion>()
            .HasIndex(p => p.Fecha);
    }
}
