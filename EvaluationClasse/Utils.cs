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
                IEnumerable<Locationparmoi> locationparmois = location.Locationparmois;
                foreach(Locationparmoi parmoi in locationparmois)
                {
                    if(parmoi.Commission == decimal.Parse("100"))
                    {
                        retour += (decimal)parmoi.Montant! * 2;
                    }
                    else
                    {
                        retour += (decimal)parmoi.Montant!;
                    }
                }
            }
            return retour;
        }

        public static Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> ChiffreParMois(IEnumerable<Location> locations)
        {
            List<Tuple<int, string>> mois = Constante.mois;
            Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> a_retourner = [];
            foreach (var location in locations)
            {
                IEnumerable<Locationparmoi> locationparmois = location.Locationparmois;
                DateOnly DateDebutLocation = new(location.Datedebut!.Value.Year, location.Datedebut!.Value.Month, 1);
                foreach (var l in locationparmois)
                {
                    DateOnly DateLocationMois = new((int)l.Annee!, (int)l.Mois!, 1);
                    if (a_retourner.ContainsKey((int)l.Annee!))
                    {
                        if (a_retourner[(int)l.Annee].ContainsKey((int)l.Mois))
                        {
                            decimal add = a_retourner[(int)l.Annee][(int)l.Mois][mois[(int)l.Mois - 1].Item2];
                            if (DateDebutLocation == DateLocationMois)
                            {
                                add += (decimal)l.Montant!*2;
                            }
                            else
                            {
                                add += (decimal)l.Montant!;
                            }


                            a_retourner[(int)l.Annee][(int)l.Mois].Remove(mois[(int)l.Mois - 1].Item2);
                            a_retourner[(int)l.Annee][(int)l.Mois].Add(mois[(int)l.Mois - 1].Item2, add);
                        }
                        else
                        {

                            Dictionary<string, decimal> final;
                            if (DateDebutLocation == DateLocationMois)
                            {
                                final = new() { { mois[(int)l.Mois! - 1].Item2, ((decimal)l.Montant!) *2} };
                            }
                            else
                            {
                                final = new() { { mois[(int)l.Mois! - 1].Item2, ((decimal)l.Montant!) } };
                            }
                            a_retourner[(int)l.Annee].Add((int)l.Mois, final);
                        }
                    }
                    //L'annee n'existe pas
                    else
                    {
                        Dictionary<string, decimal> final;
                        if (DateDebutLocation == DateLocationMois)
                        {
                            final = new() { { mois[(int)l.Mois! - 1].Item2, (decimal)l.Montant!*2 } };
                        }
                        else
                        {
                            final = new() { { mois[(int)l.Mois! - 1].Item2, ((decimal)l.Montant!)} };
                        }
                        Dictionary<int, Dictionary<string, decimal>> a_ajouter = new() { { (int)l.Mois, final } };
                        a_retourner.Add((int)l.Annee, a_ajouter);
                    }
                }

            }
            return a_retourner;
        }

        public static Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> ChiffreAffaireParMoisFiltre(Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> listemois, DateOnly debut, DateOnly fin)
        {
            Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> retour = [];

            foreach (var annee in listemois)
            {
                if (annee.Key >= debut.Year && annee.Key <= fin.Year && debut.Year == fin.Year)
                {
                    retour.Add(annee.Key, []);
                    foreach (var mois in listemois[annee.Key])
                    {
                        if (annee.Key >= debut.Year && debut.Year == fin.Year)
                        {
                            if (mois.Key >= debut.Month && mois.Key <= fin.Month)
                            {
                                retour[annee.Key].Add(mois.Key, listemois[annee.Key][mois.Key]);
                            }
                        }
                    }
                }
                else if (annee.Key >= debut.Year && annee.Key <= fin.Year && debut.Year != fin.Year)
                {
                    retour.Add(annee.Key, listemois[annee.Key]);
                }
            }
            return retour;
        }
        public static decimal ChiffreAffaireFiltre(IEnumerable<Location> locations,DateOnly debut,DateOnly fin)
        {
            decimal retour = 0;
            DateOnly DateDebutFiltre = new(debut.Year,debut.Month,1);
            DateOnly DateFinFiltre = new(fin.Year,fin.Month,1);
            foreach (Location location in locations)
            {
                foreach(var l in location.Locationparmois)
                {
                    //Date location
                    DateOnly DateLocationParMois = new((int)l.Annee!,(int)l.Mois!,1);
                    DateOnly DateDebut = new(location.Datedebut!.Value!.Year, location.Datedebut!.Value!.Month,1);
                    if(DateLocationParMois >= DateDebutFiltre && DateFinFiltre >= DateLocationParMois)
                    {
                        if (DateLocationParMois == DateDebut)
                        {
                            retour += (decimal)l.Montant! * 2;
                        }
                        else
                        {
                            retour += ((decimal)l.Montant!);
                        }
                    }
                }
            }
            return retour;
        }

        /*
            Calcul des commissions avec ou sans la duree
         */
        public static decimal GainCommission(IEnumerable<Location> locations)
        {
            decimal retour = 0;
            foreach (Location location in locations)
            {
                IEnumerable<Locationparmoi> locationparmois = location.Locationparmois;
                foreach (Locationparmoi parmoi in locationparmois)
                {

                    retour += (decimal)parmoi.Montant! * (decimal)parmoi.Commission!/100;
                }
			}
            return retour;
        }

        public static decimal GainCommissionFiltre(IEnumerable<Location> locations, DateOnly debut,DateOnly fin)
        {
            decimal retour = 0;
            DateOnly DateDebutFiltre = new(debut.Year, debut.Month,1);
			DateOnly DateFinFiltre = new(fin.Year,fin.Month,1);
			foreach (Location location in locations)
            {
                DateOnly DateDebutLocation = new(location.Datedebut!.Value.Year, location.Datedebut!.Value.Month,1);
                foreach(var l in location.Locationparmois)
                {
                    DateOnly DateLocationMois = new((int)l.Annee!,(int)l.Mois!,1);
                    if(DateLocationMois >= DateDebutFiltre && DateLocationMois <= DateFinFiltre)
                    {
                        if (DateDebutLocation == DateLocationMois)
                        {
                            retour += (decimal)l.Montant!;
                        }
                        else
                        {
                            retour += ((decimal)l.Montant!) * ((decimal)location.IdbienNavigation!.IdtypebienNavigation!.Commission!) / 100;
                        }
                    }
                }

            }
            return retour;
        }

        /*
            Calcul des commissions par mois
         */
        public static Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> GainCommissionParMois(IEnumerable<Location> locations)
        {
            List<Tuple<int, string>> mois = Constante.mois;
            Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> a_retourner = [];
            foreach (var location in locations)
            {
                IEnumerable<Locationparmoi> locationparmois = location.Locationparmois;
                DateOnly DateDebutLocation = new(location.Datedebut!.Value.Year, location.Datedebut!.Value.Month, 1);
                foreach (var l in locationparmois)
                {
                    DateOnly DateLocationMois = new((int)l.Annee!, (int)l.Mois!, 1);
                    if (a_retourner.ContainsKey((int)l.Annee!))
                    {
                        if (a_retourner[(int)l.Annee].ContainsKey((int)l.Mois))
                        {
                            decimal add = a_retourner[(int)l.Annee][(int)l.Mois][mois[(int)l.Mois - 1].Item2];
                            if (DateDebutLocation == DateLocationMois)
                            {
                                add += (decimal)l.Montant!;
                            }
                            else
                            {
                                add += (decimal)l.Montant! * (decimal)l.Commission! / 100;
                            }


                            a_retourner[(int)l.Annee][(int)l.Mois].Remove(mois[(int)l.Mois - 1].Item2);
                            a_retourner[(int)l.Annee][(int)l.Mois].Add(mois[(int)l.Mois - 1].Item2, add);
                        }
                        else
                        {
                            Dictionary<string, decimal> final = new() { { mois[(int)l.Mois! - 1].Item2, ((decimal)l.Montant! * (decimal)l.Commission!) / 100 } };
                            a_retourner[(int)l.Annee].Add((int)l.Mois, final);
                        }
                    }
                    //L'annee n'existe pas
                    else
                    {
                        Dictionary<string, decimal> final;
                        if (DateDebutLocation == DateLocationMois)
                        {
                            final = new() { { mois[(int)l.Mois! - 1].Item2, (decimal)l.Montant! } };

                        }
                        else
                        {
                            final = new() { { mois[(int)l.Mois! - 1].Item2, ((decimal)l.Montant! * (decimal)location.IdbienNavigation!.IdtypebienNavigation!.Commission!) / 100 } };
                        }
                        Dictionary<int, Dictionary<string, decimal>> a_ajouter = new() { { (int)l.Mois, final } };
                        a_retourner.Add((int)l.Annee, a_ajouter);
                    }
                }
            }
            return a_retourner;
        }

        /*
            Filtre les commissions par mois en fonction des dates
         */
        public static Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> GainCommissionFiltreMois(Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> listemois,DateOnly debut,DateOnly fin)
        {
			Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> retour = [];

			foreach (var annee in listemois)
            {
                if(annee.Key >=  debut.Year && annee.Key <= fin.Year && debut.Year == fin.Year)
                {
                    retour.Add(annee.Key,[]) ;
					foreach (var mois in listemois[annee.Key])
					{
                        DateOnly DebutMoisAnnee = new(annee.Key, mois.Key, 1);
                        if(DebutMoisAnnee >= debut && DebutMoisAnnee <= fin)
                        {
                            retour[annee.Key].Add(mois.Key, listemois[annee.Key][mois.Key]);
                        }
					}
				}
                else if(annee.Key >= debut.Year && annee.Key <= fin.Year && debut.Year != fin.Year)
                {
                    retour.Add(annee.Key, listemois[annee.Key]);
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
        public static decimal CalculChiffreAffaire(IEnumerable<Location> location,DateOnly debut,DateOnly fin)
        {
            decimal retour = 0;
            DateOnly DateDebutFiltre = new(debut.Year,debut.Month,1);
            DateOnly DateFinFiltre = new(fin.Year,fin.Month,1);

			foreach (var bien in location)
            {
                DateOnly DateDebutLocation = new(bien.Datedebut!.Value.Year,bien.Datedebut!.Value.Month,1);
                foreach (var l in bien.Locationparmois)
                {
                    DateOnly DebutLocationMois = new((int)l.Annee!, (int)l.Mois!, 1);
                    if(DebutLocationMois >= DateDebutFiltre && DebutLocationMois <= DateFinFiltre)
                    {
                        if(DateDebutLocation == DebutLocationMois)
                        {
                            retour += (decimal)l.Montant!;
                        }
                        else
                        {

                            retour += ((decimal)l.Montant!) - (((decimal)l.Montant!) * ((decimal)l.Commission!)/100);
                        }
                    }
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
