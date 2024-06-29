using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class ProprietaireController : Controller
    {
        private readonly IBienService BienService;
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly IClientService ClientService;
        public ProprietaireController(IBienService bienService,IHttpContextAccessor httpContextAccessor,IClientService clientService)
        {
            BienService = bienService;
            HttpContextAccessor = httpContextAccessor;
            ClientService = clientService;
        }
        public async Task<IActionResult> Index()
        {
            if(HttpContextAccessor.HttpContext!.Session.GetString("id") == null) return RedirectToAction("Index","Home");
            string t = HttpContextAccessor.HttpContext!.Session.GetString("id")!;
            Client? proprietaire = await ClientService.GetClientByIdAsync(t)!;
			IEnumerable<Bien> liste_bien = await BienService.SelectBienByProprietaireAsync(proprietaire!);
            ViewData["list"] = liste_bien;
            return View();
        }
    }
}
