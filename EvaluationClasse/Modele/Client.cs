using System;
using System.Collections.Generic;

namespace EvaluationClasse;

public partial class Client
{
    public string Idclient { get; set; } = null!;

    public string? Numeroclient { get; set; }

    public string? Emailclient { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
