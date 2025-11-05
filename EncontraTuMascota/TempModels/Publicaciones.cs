using System;
using System.Collections.Generic;

namespace EncontraTuMascota.TempModels;

public partial class Publicaciones
{
    public int Id { get; set; }

    public int MascotaId { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Contacto { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string? UsuarioId { get; set; }

    public bool Cerrada { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string? Resolucion { get; set; }

    public virtual Mascotas Mascota { get; set; } = null!;

    public virtual AspNetUsers? Usuario { get; set; }
}
