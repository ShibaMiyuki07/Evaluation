using Evaluation.Log.Interface;

namespace Evaluation.Models
{
    public class ErrorModel
    {
        public static void HandleError(ILoggerManager logger,Exception e,string classe,string fonction)
        {
            logger.LogError($"{classe}.{fonction} : {e.Message} - {e.StackTrace}");
            throw e;
        }
    }
}
