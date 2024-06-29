using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationClasse
{
    public class Utils
    {

        public static decimal ChiffreAffaire(IEnumerable<Location> locations)
        {
            decimal retour = 0;
            foreach (Location location in locations)
            {
                retour += (decimal)((decimal)location.IdbienNavigation!.Loyer! * location.Duree)!;
            }
            return retour;
        }

        public static decimal GainCommission(IEnumerable<Location> locations,bool avecDuree = true)
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

        public static List<Tuple<int,string,decimal>> GainCommissionParMois(IEnumerable<Location> locations)
        {
            List<Tuple<int,string,decimal>> retour = [];
            List<Tuple<int, string>> mois = Constante.mois;
            for (int i = 0;i<mois.Count;i++)
            {
                decimal chiffre_mois = 0;
                IEnumerable<Location> liste_mois = locations.Where(l => l.Datedebut!.Value.Month == mois[i].Item1).ToList();
                chiffre_mois += GainCommission(liste_mois,false);
                IEnumerable<Location> liste_en_cours = locations.Where(l => ((l.Datedebut!.Value.Month + l.Duree) > mois[i].Item1) && l.Datedebut!.Value.Month != mois[i].Item1).ToList();
                chiffre_mois += GainCommission(liste_en_cours, false);

                retour.Add(new Tuple<int,string, decimal>(mois[i].Item1,mois[i].Item2,chiffre_mois));
            }
            return retour;
        }

        public static List<Tuple<int,string,decimal>> GainCommissionFiltreMois(List<Tuple<int,string,decimal>> listemois,DateOnly debut,DateOnly fin)
        {
            List<Tuple<int, string, decimal>> retour = [];

			foreach (var l in listemois)
            {
                if(l.Item1 >= debut.Month && l.Item1 <= fin.Month)
                {
                    retour.Add(l);
                }
            }
            return retour;
        }

		#region Utils Location
		public static decimal Commission(Bien bien)
        {
            return (decimal)(bien.Loyer! * bien.IdtypebienNavigation!.Commission!)/ 100;
        }

        public static int Duree(DateOnly debut,DateOnly fin,int d)
        {
            int duree = 0;
            for(int i=debut.Month;i<=fin.Month;i++)
            {
                if(duree <= d)
                {
                    duree++;
                }
            }
            return duree;
        }

        public static decimal CalculChiffreAffaire(IEnumerable<Location> location,DateOnly fin)
        {
            decimal retour = 0;
            foreach (var bien in location)
            {
                retour += (decimal)((bien.IdbienNavigation!.Loyer - Commission(bien.IdbienNavigation)) * Duree((DateOnly)bien.Datedebut!,fin,(int)bien.Duree!))!;
            }
            return retour;
        }
        #endregion

        #region Utils Paye
        /*
            Change la liste des paye en dictionnaire
         */
        public static Dictionary<string, Dictionary<int, Dictionary<int, string>>> ListPayeToDictionnary(IEnumerable<Paye> liste)
        {
            Dictionary<string, Dictionary<int, Dictionary<int, string>>> retour = [];
            foreach(Paye paye in liste)
            {
                if(!retour.ContainsKey(paye.Idlocation))
                {
                    retour[paye.Idlocation] = [];
                }
                Dictionary<int, string> last = new Dictionary<int, string> { { (int)paye.Moispaye!, "Paye" } };
                retour[paye.Idlocation!].Add((int)paye.Anneepaye!, last );
            }
            return retour;
        }

        public static List<Tuple<string,Location,string>> Payes(IEnumerable<Location> locations,Dictionary<string,Dictionary<int,Dictionary<int,string>>> paye,DateOnly fin)
        {
            List<Tuple<string,Location, string>> retour = [];
            foreach (var location in locations)
            {
                int duree = Duree(location.Datedebut!.Value, fin,(int)location.Duree!);
                for(int i=0;i<duree; i++)
                {
                    string status = "Non Paye";
                    if(paye.ContainsKey(location.Idlocation))
                    {
                        if (paye[location.Idlocation].ContainsKey(location.Datedebut.Value.Year))
                        {
                            if (paye[location.Idlocation][location.Datedebut.Value.Year].ContainsKey(location.Datedebut.Value.Month))
                            {
                                status = paye[location.Idlocation][location.Datedebut.Value.Year][location.Datedebut.Value.Month];
                            }
                        }
                    }
                    retour.Add(new Tuple<string,Location, string>(Constante.mois[location.Datedebut.Value.Month].Item2,location, status));
                    location.Datedebut = location.Datedebut.Value.AddMonths(1);
                }
            }
            return retour;
        }
        #endregion

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
                return false;
            }
            if (!VerifyEmail(str!))
            {
                return false;
            }
            return true;
        }

        public static bool CheckNumero(string str)
        { 
            try
            {
                int.Parse(str);
            }
            catch { return false; }
            return true;
        }
    }
}
