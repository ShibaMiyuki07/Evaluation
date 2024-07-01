using EvaluationClasse.Modele;
using EvaluationClasse.Utiles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationClasse
{
    public class Utils
    {

        public static decimal ChiffreAffaireSansDate(IEnumerable<Location> locations)
        {
            decimal retour = 0;
            foreach (Location location in locations)
            {
                retour += (decimal)((decimal)location.IdbienNavigation!.Loyer! * location.Duree)!;
            }
            return retour;
        }

        public static decimal ChiffreAffaireFiltre(IEnumerable<Location> locations,DateOnly debut,DateOnly fin)
        {
            decimal retour = 0;
            foreach (Location location in locations)
            {
                DateOnly final = location.Datedebut!.Value.AddMonths((int)location.Duree!);
                DateOnly debutautre = location.Datedebut.Value;
                if (location.Datedebut < debut)
                {
                    debutautre = debut;
                }
                int duree = 0;
                UtilsBien.SetDuree(location, debut, fin, final);
                duree = Duree(debutautre, fin, (int)location.Duree!);
                retour += (decimal)(location.IdbienNavigation!.Loyer * duree)!;
            }
            return retour;
        }

        /*
            Calcul des commissions avec ou sans la duree
         */
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

        public static decimal GainCommissionFiltre(IEnumerable<Location> locations, DateOnly debut,DateOnly fin)
        {
            decimal retour = 0;
            foreach (Location location in locations)
            {
                DateOnly final = location.Datedebut!.Value.AddMonths((int)location.Duree!);
                if (location.Datedebut < debut)
                {
                    location.Datedebut = debut;
                }
                int duree = 0;
                UtilsBien.SetDuree(location, debut, fin, final);
                duree = Duree(location.Datedebut!.Value, fin, (int)location.Duree!);
                retour += (decimal)((((decimal)location.IdbienNavigation!.Loyer! * (decimal)location.IdbienNavigation.IdtypebienNavigation!.Commission!) / 100) * duree)!;

            }
            return retour;
        }

        /*
            Calcul des commissions par mois
         */
        public static List<Tuple<int,string,decimal>> GainCommissionParMois(IEnumerable<Location> locations)
        {
            List<Tuple<int, string, decimal>> retour = [];
            List<Tuple<int, string>> mois = Constante.mois;

            IEnumerable<MoisGain> LocationDateDebut = locations.GroupBy(l => l.Datedebut!.Value!.Month).Select(x => new MoisGain { Mois = x.Key, Amount = x.Sum(l => (decimal)l.IdbienNavigation!.Loyer!*(decimal)l.IdbienNavigation.IdtypebienNavigation!.Commission!)/100}).ToList();
            for(int i=0; i<mois.Count;i++)
            {
                MoisGain MoisGain = LocationDateDebut.Where(x => x.Mois == mois[i].Item1).FirstOrDefault()!;

                /*
                    Get All A Payer (datedebut + duree) - mois > 0
                 */
                IEnumerable<Location> MoisEnCours = locations.Where(x => (x.Datedebut!.Value.Month < mois[i].Item1 && (x.Datedebut.Value!.AddMonths((int) x.Duree!).Month - mois[i].Item1 >0))).ToList()!;
                
                decimal to_add = 0;
                if(!MoisEnCours.Any() && MoisGain != null)
                {
                    to_add = MoisGain.Amount;
                }
                else if(MoisGain == null&& MoisEnCours.Any())
                {
                    foreach(Location location in MoisEnCours)
                    {
                        to_add += ((decimal)location.IdbienNavigation!.Loyer! * (decimal)location.IdbienNavigation.IdtypebienNavigation!.Commission!)/100;
                    }
                }
                else if(MoisEnCours.Any() && MoisGain != null)
                {
                    foreach(Location location in MoisEnCours)
                    {
                        to_add += (decimal)location.IdbienNavigation!.Loyer! * (decimal)location.IdbienNavigation.IdtypebienNavigation!.Commission!/100;
                    }
                    to_add += MoisGain.Amount;
                }
                retour.Add(new Tuple<int, string, decimal>(mois[i].Item1, mois[i].Item2,to_add));
            }

            return retour;
        }

        /*
            Filtre les commissions par mois en fonction des dates
         */
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
                if(duree < d)
                {
                    duree++;
                }
            }
            return duree;
        }

        /*
            Calcul des chiffres d'affaires 
         */
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
