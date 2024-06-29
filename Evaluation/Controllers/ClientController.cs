using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class ClientController(IHttpContextAccessor httpContextAccessor,IPayeService payeService,IClientService service,ILocationService locationService) : Controller
    {
        private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;
        private readonly IPayeService _PayeService = payeService;
        private readonly IClientService _ClientService = service;
        private readonly ILocationService _LocationService = locationService;

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> Loyer(DateOnly debut,DateOnly fin) 
        {
            if (_HttpContextAccessor.HttpContext!.Session.GetString("id") == null) return RedirectToAction("Index", "Home");
            string t = _HttpContextAccessor.HttpContext!.Session.GetString("id")!;
            Client? client = await _ClientService.GetClientByIdAsync(t)!;
            IEnumerable<Paye> liste_paye = await _PayeService.SelectByClientAsync(client!);
            Dictionary<string, Dictionary<int, Dictionary<int, string>>> dicPaye = Utils.ListPayeToDictionnary(liste_paye);
            IEnumerable<Location> locations = await _LocationService.SerlectByIdAndDebut(client!,debut);
            List<Tuple<string,Location, string>> final = Utils.Payes(locations,dicPaye,fin);
            ViewData["liste_final"] = final;
            return View("Index");
        }

    }
}
