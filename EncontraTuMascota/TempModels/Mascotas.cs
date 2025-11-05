using System;
using System.Collections.Generic;

namespace EncontraTuMascota.TempModels;

public partial class Mascotas
{
    public int Id { get; set; }

    public string FotoUrl { get; set; } = null!;

    public int Sexo { get; set; }

    public int Raza { get; set; }

    public string Ubicacion { get; set; } = null!;

    public string NombreContacto { get; set; } = null!;

    public string TelefonoContacto { get; set; } = null!;

    public string EmailContacto { get; set; } = null!;

    public DateTime FechaPublicacion { get; set; }

    public virtual ICollection<Publicaciones> Publicaciones { get; set; } = new List<Publicaciones>();
}
