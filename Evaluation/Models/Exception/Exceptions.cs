namespace Evaluation.Models.Exceptions
{
    public class CsvException : Exception
    {
        public CsvException(string e) { throw new Exception(e);  }

        public CsvException() : base($"Le csv envoyé contient des lignes non valide") { }
    }

    public class IEnumerableException : Exception
    {
        public IEnumerableException(string e){ throw new Exception(e); }

        public IEnumerableException() : base($"La liste contient un probleme") {}
    }
}

