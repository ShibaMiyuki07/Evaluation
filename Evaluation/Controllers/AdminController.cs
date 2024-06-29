using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class AdminController(IHttpContextAccessor httpContextAccessor, ILocationService LocationService,IAdminService adminService) : Controller
    {
        private readonly IHttpContextAccessor ContextAccessor = httpContextAccessor;
        private readonly ILocationService LocationService = LocationService;
        private readonly IAdminService adminService = adminService;


        //Chiffre d'affaire = location totale
        //Gains = commission
        /*
            Index avec le total des gains , les gains par mois, chiffre d'affaire sans commission
         */
        public async Task<IActionResult> Index()
        {
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();

            decimal TotalChiffreAffaire = Utils.ChiffreAffaireSansDate(locations);
            ViewData["TotalChiffreAffaire"] = TotalChiffreAffaire;


            decimal TotalCommission = Utils.GainCommission(locations);
            ViewData["TotalCommission"] = TotalCommission;


            List<Tuple<int,string,decimal>> GainParMois = Utils.GainCommissionParMois(locations);
            ViewData["GainParMois"] = GainParMois;


            decimal ChiffreSansCommission = Math.Abs(TotalCommission - TotalChiffreAffaire);
            ViewData["ChiffreSansCommission"] = ChiffreSansCommission;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(DateOnly debut,DateOnly fin)
        {
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectByDateDebut(debut);

			decimal TotalChiffreAffaireFiltre = Utils.ChiffreAffaireFiltre(locations,debut,fin);
			ViewData["TotalChiffreAffaire"] = TotalChiffreAffaireFiltre;


			decimal TotalCommissionFiltre = Utils.GainCommissionFiltre(locations,debut,fin);
			ViewData["TotalCommission"] = TotalCommissionFiltre;


			List<Tuple<int, string, decimal>> GainParMoisFiltre = Utils.GainCommissionParMois(locations);
            GainParMoisFiltre = Utils.GainCommissionFiltreMois(GainParMoisFiltre, debut,fin);
            ViewData["GainParMois"] = GainParMoisFiltre;

            decimal chiffresanscommission = Math.Abs(TotalChiffreAffaireFiltre - TotalCommissionFiltre);
            ViewData["ChiffreSansCommission"] = chiffresanscommission;
			return View("Index", ViewData);
		}

        public async Task<IActionResult> DeleteAll()
        {
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            await adminService.DeleteAll();
            return RedirectToAction("Index","Home");
        }

    }
}
