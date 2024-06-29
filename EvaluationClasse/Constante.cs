using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationClasse
{
    internal class Constante
    {
        public static List<string> mailExtension = [".com", ".fr", ".mg", ".de", ".org", ".net"];

        public static List<Tuple<int, string>> mois = [new Tuple<int,string>(1, "Janvier"),
            new Tuple<int, string>(2, "Fevrier"), 
            new Tuple<int, string>(3, "Mars"), 
            new Tuple<int, string>(4, "Avril"),
            new Tuple<int, string>(5, "Mai"), 
            new Tuple<int, string>(6, "Juin"),
            new Tuple<int, string>(7, "Juillet"),
            new Tuple<int, string>(8, "Aout"),
            new Tuple<int, string>(9, "Septembre"),
            new Tuple<int, string>(10, "Octobre"),
            new Tuple<int, string>(11, "Novembre"),
            new Tuple<int, string>(12, "Decembre")];
    }
}
