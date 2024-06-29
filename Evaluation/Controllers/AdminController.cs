using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpContextAccessor ContextAccessor;
        private readonly ILocationService LocationService;

        public AdminController(IHttpContextAccessor httpContextAccessor, ILocationService LocationService)
        {
            ContextAccessor = httpContextAccessor;
            this.LocationService = LocationService;
        }
        public async Task<IActionResult> Index()
        {
            if(ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index","Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();
            decimal totalChiffre = Utils.ChiffreAffaireCommission(locations);
            ViewData["total"] = totalChiffre;
            return View();
        }
    }
}
