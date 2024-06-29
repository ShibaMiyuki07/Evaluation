using System;
using System.Collections.Generic;

namespace EvaluationClasse;

public partial class Typebien
{
    public string Idtypebien { get; set; } = null!;

    public string? Type { get; set; }

    public decimal? Commission { get; set; }

    public virtual ICollection<Bien> Biens { get; set; } = new List<Bien>();
}
