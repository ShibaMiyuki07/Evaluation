using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
