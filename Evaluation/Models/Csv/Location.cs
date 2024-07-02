using CsvHelper.Configuration.Attributes;

namespace Evaluaton.Models.Csv
{
    public class Location
    {
        [Name("reference")]
        public string Reference { get; set; } = string.Empty;

        [Name("Date debut")]
        public string DateDebut {  get; set; }

        [Name("duree mois")]
        public int Duree {  get; set; } 
        public string Client { get; set; } = string.Empty;
    }
}
