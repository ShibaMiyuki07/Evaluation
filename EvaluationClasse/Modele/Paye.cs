using System;
using System.Collections.Generic;

namespace EvaluationClasse;

public partial class Paye
{
    public string Idpaye { get; set; } = null!;

    public string? Idlocation { get; set; }

    public int? Moispaye { get; set; }

    public int? Anneepaye { get; set; }

    public virtual Location? IdlocationNavigation { get; set; }
}
