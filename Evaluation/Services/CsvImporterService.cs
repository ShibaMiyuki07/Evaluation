using CsvHelper;
using CsvHelper.Configuration;
using Evaluation.Models.Exceptions;
using System.Globalization;

namespace Evaluation.Services
{
    public class CsvImporterService<T>
    {
        private IEnumerable<T>? line;

        private readonly CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header!.ToLower(),
            MissingFieldFound = null,
            HeaderValidated = null
        };

        public IEnumerable<T> Import(string filename)
        {
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

        public IEnumerable<T> ImportFromIFormFile(IFormFile file)
        {

            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded or file is empty");
            }
            try
            {
                var stream = file.OpenReadStream();
                using (var reader = new StreamReader(stream))
                {
                    var csv = new CsvReader(reader, config);
                    line = csv.GetRecords<T>().ToList();
                }
            }
            catch (Exception ex) { throw new CsvException(ex.Message); }
            if (!line.Any()) throw new IEnumerableException("La liste est vide");

            return line;
        }
    }
}
