using Evaluation.Context;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor ContextAccessor;
        private readonly EvaluationsContext EvaluationContext;

        public AdminController(IHttpContextAccessor httpContextAccessor, EvaluationsContext evaluationContext)
        {
            ContextAccessor = httpContextAccessor;
            EvaluationContext = evaluationContext;
        }
        public IActionResult Index()
        {
            
            return View();
        }
    }
}
