using CsvHelper;
using CsvHelper.Configuration;
using Evaluation.Log.Interface;
using Evaluation.Models;
using Evaluation.Models.Exceptions;
using System.Globalization;

namespace Evaluation.Services
{
    public class CsvImporterService<T>(ILoggerManager logger)
    {
        private IEnumerable<T>? line;
        private readonly ILoggerManager _logger = logger;

        private readonly CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header!.ToLower(),
            MissingFieldFound = null,
            HeaderValidated = null
        };

        public async Task<IEnumerable<T>> Import(string filename)
        {
            return await Task.Run(() => {
                try
                {
                    using (var fs = new StreamReader(filename))
                    {
                        line = RetrieveCsv(fs);
                    }
                    if (!line.Any())
                    {
                        throw new IEnumerableException("La liste est vide");
                    }
                }
                catch (IEnumerableException ex)
                {
                    ErrorModel.HandleError(_logger, ex, "CsvImporterService", "Import");
                }
                catch (Exception ex)
                {
                    ErrorModel.HandleError(_logger, ex, "CsvImporterService", "Import");
                }

                return line!;
            });
        }

        public async Task<IEnumerable<T>> ImportFromIFormFile(IFormFile file)
        {
            return await Task.Run(() => {
                try
                {
                    if (file == null || file.Length == 0)
                    {
                        throw new ArgumentException("No file uploaded or file is empty");
                    }
                    var stream = file.OpenReadStream();
                    using (var reader = new StreamReader(stream))
                    {
                        line = RetrieveCsv(reader);
                    }

                    if (!line.Any()) throw new IEnumerableException("La liste est vide");
                }
                catch (IEnumerableException ex)
                {
                    ErrorModel.HandleError(_logger, ex, "CsvImporterService", "ImportFromIFormFile");
                }
                catch (ArgumentException ex)
                {
                    ErrorModel.HandleError(_logger, ex, "CsvImporterService", "ImportFromIFormFile");
                }
                catch (Exception ex)
                {
                    ErrorModel.HandleError(_logger, ex, "CsvImporterService", "ImportFromIFormFile");
                }

                return line!;
            });
        }

        private IEnumerable<T> RetrieveCsv(StreamReader reader)
        {
            var csv = new CsvReader(reader, config);
            line = csv.GetRecords<T>().ToList();
            return line;
        }
    }
}
