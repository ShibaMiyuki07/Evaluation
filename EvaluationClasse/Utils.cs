using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationClasse
{
    public class Utils
    {

        public static decimal ChiffreAffaireCommission(IEnumerable<Location> locations,bool avecDuree = true)
        {
            decimal retour = 0;
            foreach (Location location in locations)
            {
                if (avecDuree)
                {
                    retour += (decimal)((((decimal)location.IdbienNavigation!.Loyer! * (decimal)location.IdbienNavigation.IdtypebienNavigation!.Commission!) / 100) * location.Duree)!;
                }
                else retour += ((((decimal)location.IdbienNavigation!.Loyer! * (decimal)location.IdbienNavigation.IdtypebienNavigation!.Commission!) / 100))!;

			}
            return retour;
        }

        public static List<Tuple<string,decimal>> ChiffreAffaireCommissionParMois(IEnumerable<Location> locations)
        {
            List<Tuple<string,decimal>> retour = [];
            List<Tuple<int, string>> mois = Constante.mois;
            for (int i = 0;i<mois.Count;i++)
            {
                decimal chiffre_mois = 0;
                IEnumerable<Location> liste_mois = locations.Where(l => l.Datedebut!.Value.Month == mois[i].Item1).ToList();
                chiffre_mois += ChiffreAffaireCommission(locations,false);
                IEnumerable<Location> liste_en_cours = locations.Where(l => (l.Datedebut!.Value.Month + l.Duree) <= mois[i].Item1).ToList();
                chiffre_mois += ChiffreAffaireCommission(locations, false);

                retour.Add(new Tuple<string, decimal>(mois[i].Item2,chiffre_mois));
            }
            return retour;
        }

        private static int CountArobase(string str)
        {
            return str!.ToCharArray().Count(c => c == '@');
        }

        private static bool VerifyEmail(string str)
        {
            foreach (string c in Constante.mailExtension)
            {
                if (str.EndsWith(c))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckEmail(string str)
        {
            int nbr_arobase = CountArobase(str!);
            if (nbr_arobase != 1)
            {
                throw new Exception("Email non valide");
            }
            if (!VerifyEmail(str!))
            {
                throw new Exception("Le nom de domaine de l'email est invalide");
            }
            return true;
        }

        public static bool CheckNumero(string str)
        { 
            try
            {
                int.Parse(str);
            }
            catch { throw; }
            return true;
        }
    }
}
