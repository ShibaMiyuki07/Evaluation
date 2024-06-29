using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationClasse
{
    public class Utils
    {
        private static int countArobase(string str)
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
            int nbr_arobase = countArobase(str!);
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

        //public static bool CheckNumero()
    }
}
