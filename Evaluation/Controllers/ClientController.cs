using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
