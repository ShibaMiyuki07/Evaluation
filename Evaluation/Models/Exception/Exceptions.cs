namespace Evaluation.Models.Exceptions
{
    public class CsvException : Exception
    {
        public CsvException(string e) { throw new Exception(e);  }
    }

    public class IEnumerableException : Exception
    {
        public IEnumerableException(string e){ throw new Exception(e); }
    }
}

