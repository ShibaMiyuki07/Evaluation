using System;
using System.Collections.Generic;

namespace EvaluationClasse;

public partial class Bien
{
    public string Idbien { get; set; } = null!;

    public string? Nombien { get; set; }

    public string? Description { get; set; }

    public string? Region { get; set; }

    public decimal? Loyer { get; set; }

    public string? Photos { get; set; }

    public string? Idproprietaire { get; set; }

    public string? Idtypebien { get; set; }

    public virtual Client? IdproprietaireNavigation { get; set; }

    public virtual Typebien? IdtypebienNavigation { get; set; }

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
