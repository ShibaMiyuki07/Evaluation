using CsvHelper;
using CsvHelper.Configuration;
using Evaluation.Models.Exceptions;
using System.Globalization;

namespace Evaluation.Services
{
    public class CsvImporterService<T>
    {
        private IEnumerable<T>? line;

        public IEnumerable<T> Import(string filename)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header!.ToLower(),
                MissingFieldFound = null,
                HeaderValidated = null
            };
            try
            {
                using (var fs = new StreamReader(filename))
                {
                    var csv = new CsvReader(fs, config);
                    line = csv.GetRecords<T>().ToList();
                }
            }
            catch (Exception ex) { throw new CsvException(ex.Message); }
            if (!line.Any()) { throw new IEnumerableException("La liste est vide"); }
            return line;
        }
    }
}
