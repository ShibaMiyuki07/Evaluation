using System;
using System.Collections.Generic;

namespace EvaluationClasse;

public partial class Locationparmoi
{
    public string Idlocationparmois { get; set; } = null!;

    public int? Mois { get; set; }

    public int? Annee { get; set; }

    public decimal? Montant { get; set; }

    public string? Idlocation { get; set; }

    public virtual Location? IdlocationNavigation { get; set; }
}
