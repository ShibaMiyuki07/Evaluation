using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationClasse.Utiles
{
    public class UtilsBien
    {
        #region Utils Paye
        /*
            Change la liste des paye en dictionnaire
         */
        //public static Dictionary<string, Dictionary<int, Dictionary<int, string>>> ListPayeToDictionnary(IEnumerable<Paye> liste)
        //{
        //    Dictionary<string, Dictionary<int, Dictionary<int, string>>> retour = [];
        //    foreach (Paye paye in liste)
        //    {
        //        if (!retour.TryGetValue(paye.Idlocation, out Dictionary<int, Dictionary<int, string>>? value))
        //        {
        //            value = ([]);
        //            retour[paye.Idlocation] = value;
        //        }
        //        Dictionary<int, string> last = new() { { (int)paye.Moispaye!, "Paye" } };
        //        value.Add((int)paye.Anneepaye!, last);
        //    }
        //    return retour;
        //}

        /**
         * Check les payes
         * Paye jusqu'a maintenant
         */
        public static List<Tuple<string,int,int, Location, string>> Payes(IEnumerable<Location> locations, DateOnly debut, DateOnly fin)
        {
            List<Tuple<string,int,int, Location, string>> retour = [];
            DateOnly DateDebutFiltre = new(debut.Year,debut.Month,1);
            DateOnly DateFinFiltre = new(fin.Year,fin.Month,1);
            foreach (var location in locations)
            {
                foreach(var lpm in location.Locationparmois)
                {
                    DateOnly DateLocationParMois = new((int)lpm.Annee!,(int)lpm.Mois!,1) ;
                    if(DateLocationParMois >=DateDebutFiltre && DateLocationParMois<=DateFinFiltre)
                    {
                        string status = "Non Paye";
                        if (location.Datedebut!.Value.Month <= DateTime.Now.Month && location.Datedebut.Value.Year <= DateTime.Now.Year)
                        {
                            status = "Paye";
                        }

                        retour.Add(new Tuple<string,int,int, Location, string>(Constante.mois[DateLocationParMois.Month - 1].Item2, DateLocationParMois.Month,DateLocationParMois.Year, location, status));
                    }
                }
            }
            return retour;
        }

        public static void SetDuree(Location location, DateOnly debut, DateOnly fin, DateOnly final)
        {
            if (location.Datedebut <= debut && debut < final)
            {
                location.Datedebut = debut;
                if (final < fin)
                {
                    location.Duree = (final.Month - debut.Month);
                }
                else { location.Duree = ( fin.Month - debut.Month)+1; }
            }
        }

        //public static string GetStatus(Location location, Dictionary<string, Dictionary<int, Dictionary<int, string>>> paye)
        //{
        //    string status = "Non Payé";
        //    if (paye.ContainsKey(location.Idlocation))
        //    {
        //        if (paye[location.Idlocation].ContainsKey(location.Datedebut!.Value.Year))
        //        {
        //            if (paye[location.Idlocation][location.Datedebut.Value.Year].ContainsKey(location.Datedebut.Value.Month))
        //            {
        //                status = paye[location.Idlocation][location.Datedebut.Value.Year][location.Datedebut.Value.Month];
        //            }
        //        }
        //    }
        //    return status;
        //}
        #endregion
    }
}
