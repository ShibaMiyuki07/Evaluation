using Evaluation.Services.Interface;
using EvaluationClasse;
using EvaluationClasse.Utiles;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class ClientController(IHttpContextAccessor httpContextAccessor,IClientService service,ILocationService locationService) : Controller
    {
        private readonly IHttpContextAccessor _HttpContextAccessor = httpContextAccessor;
        private readonly IClientService _ClientService = service;
        private readonly ILocationService _LocationService = locationService;

        public async Task<IActionResult> Index()
        {
            if (_HttpContextAccessor.HttpContext!.Session.GetString("id") == null || _HttpContextAccessor.HttpContext!.Session.GetString("id") == string.Empty) return RedirectToAction("Index", "Home");
            string t = _HttpContextAccessor.HttpContext!.Session.GetString("id")!;
            Client? client = await _ClientService.GetClientByIdAsync(t)!;
            if (client.Emailclient == null) return RedirectToAction("Index", "Home");
            return await Task.Run(() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> Loyer(DateOnly debut,DateOnly fin) 
        {
            //Check User identity
            if (_HttpContextAccessor.HttpContext!.Session.GetString("id") == null || _HttpContextAccessor.HttpContext!.Session.GetString("id") == string.Empty) return RedirectToAction("Index", "Home");
            string t = _HttpContextAccessor.HttpContext!.Session.GetString("id")!;
            Client? client = await _ClientService.GetClientByIdAsync(t)!;
            if(client!.Emailclient == null) return RedirectToAction("Index", "Home");


            IEnumerable<Location> locations = await _LocationService.SelectByIdAndDebut(client!,debut);

            List<Tuple<string,int,int,Location, string>> final = UtilsBien.Payes(locations,debut,fin);
            ViewData["liste_final"] = final;
            return View("Index");
        }

    }
}
