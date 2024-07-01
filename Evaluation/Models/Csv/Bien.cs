using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
namespace Evaluaton.Models.Csv
{
    public class Bien
    {
        [Name("reference")]
        public string Reference { get; set; } = string.Empty;

        [Name("nom")]
        public string Nom { get; set; } = string.Empty;
        [Name("description")]
        public string Description { get; set; } = string.Empty;
        [Name("type")]
        public string Type {  get; set; } = string.Empty;
        [Name("region")]
        public string Region {  get; set; } = string.Empty;

        [Name("loyer mensuel")]
        public decimal Loyer { get; set; } = 0;
        [Name("proprietaire")]
        public string Proprietaire { get; set; } = string.Empty;
    }
}
