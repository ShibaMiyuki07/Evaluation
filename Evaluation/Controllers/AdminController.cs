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
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();
            decimal totalChiffre = Utils.ChiffreAffaireCommission(locations);
            ViewData["total"] = totalChiffre;
            List<Tuple<int,string,decimal>> chiffreparmois = Utils.ChiffreAffaireCommissionParMois(locations);
            ViewData["chiffreparmois"] = chiffreparmois;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(DateOnly debut,DateOnly fin)
        {
			IEnumerable<Location> locations = await LocationService.SelectAllAsync();
			decimal totalChiffre = Utils.ChiffreAffaireCommission(locations);
			ViewData["total"] = totalChiffre;
			List<Tuple<int,string, decimal>> chiffreparmois = Utils.ChiffreAffaireCommissionParMois(locations);
			chiffreparmois = Utils.ChiffreAffaireFiltreMois(chiffreparmois,debut,fin);
			ViewData["chiffreparmois"] = chiffreparmois;
			return View("Index", ViewData);
		}

    }
}
