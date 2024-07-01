using System;
using System.Collections.Generic;

namespace EvaluationClasse;

public partial class Location
{
    public string Idlocation { get; set; } = null!;

    public string? Idclient { get; set; }

    public int? Duree { get; set; }

    public DateOnly? Datedebut { get; set; }

    public string? Idbien { get; set; }

    public virtual Bien? IdbienNavigation { get; set; }

    public virtual Client? IdclientNavigation { get; set; }

    public virtual ICollection<Locationparmoi> Locationparmois { get; set; } = new List<Locationparmoi>();

    public virtual ICollection<Paye> Payes { get; set; } = new List<Paye>();
}
