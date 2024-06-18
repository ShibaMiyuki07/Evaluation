using System.ComponentModel.DataAnnotations;

namespace Evaluation.Models
{
    public class Joueur
    {
        public string IdJoueur { get; set; } = String.Empty;
        public string Nom { get; set; } = String.Empty;
        public string Prenom {  get; set; } = String.Empty;
        public int Taille { get; set; } = 0;
        public int Attaquant { get; set; } = 0;
        public int Milieu { get; set; } = 0;
        public int Defenseur { get; set; } = 0;
        public int Gardien { get; set; } = 0;
        public string Nationalite {  get; set; } = String.Empty;
        public string Club {  get; set; } = String.Empty;
        public int Physique { get; set; } = 0;
        public int Vitesse { get; set; } = 0;
        public int Passe { get; set; } = 0;
        public int Tir { get; set; } = 0;
        public int Dribble { get; set; } = 0;
        public int Defense { get; set; } = 0;
        public int Image { get; set; } = 0;

    }
}
