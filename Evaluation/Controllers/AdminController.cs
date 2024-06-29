using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class AdminController(IHttpContextAccessor httpContextAccessor, ILocationService LocationService) : Controller
    {
        private readonly IHttpContextAccessor ContextAccessor = httpContextAccessor;
        private readonly ILocationService LocationService = LocationService;

        public async Task<IActionResult> Index()
        {
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();

            decimal chiffreAffaire = Utils.ChiffreAffaire(locations);
            ViewData["chiffreaffaire"] = chiffreAffaire;
            decimal totalGain = Utils.GainCommission(locations);
            ViewData["gainTotal"] = totalGain;
            List<Tuple<int,string,decimal>> GainParMois = Utils.GainCommissionParMois(locations);
            ViewData["gainparmois"] = GainParMois;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(DateOnly debut,DateOnly fin)
        {
			IEnumerable<Location> locations = await LocationService.SelectAllAsync();
			decimal chiffreAffaire = Utils.ChiffreAffaire(locations);
			ViewData["chiffreaffaire"] = chiffreAffaire;
			decimal totalGain = Utils.GainCommission(locations);
			ViewData["gainTotal"] = totalGain;
			List<Tuple<int, string, decimal>> GainParMois = Utils.GainCommissionParMois(locations);
			GainParMois = Utils.GainCommissionFiltreMois(GainParMois, debut,fin);
			ViewData["gainparmois"] = GainParMois;
			return View("Index", ViewData);
		}

    }
}
