using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class AdminController(IHttpContextAccessor httpContextAccessor,
        ILocationService LocationService,
        IAdminService AdminService,
        IBienService BienService,
        IClientService ClientService) : Controller
    {
        private readonly IHttpContextAccessor ContextAccessor = httpContextAccessor;
        private readonly ILocationService LocationService = LocationService;
        private readonly IAdminService AdminService = AdminService;
        private readonly IBienService BienService = BienService;
        private readonly IClientService ClientService = ClientService;

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

            decimal TotalChiffreAffaire = decimal.Round(Utils.ChiffreAffaireSansDate(locations),2);
            ViewData["TotalChiffreAffaire"] = TotalChiffreAffaire;


            decimal TotalCommission = decimal.Round(Utils.GainCommission(locations), 2);
            ViewData["TotalCommission"] = TotalCommission;


			Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> GainParMois = Utils.GainCommissionParMois(locations);
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


			Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> GainParMoisFiltre = Utils.GainCommissionParMois(locations);
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
            await AdminService.DeleteAll();
            return RedirectToAction("Index","Home");
        }


        public async Task<IActionResult> Location()
        {

			if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
			{
				return RedirectToAction("Index", "Home");
			}
			IEnumerable<Bien> ListeBien = await BienService.SelectAllAsync();
            IEnumerable<Client> ListeClient = await ClientService.GetAllClient();
            ViewData["ListeBien"] = ListeBien;
            ViewData["ListeClient"] = ListeClient;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Location(string idbien,string idclient,string datedebut,string duree)
        {

			if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
			{
				return RedirectToAction("Index", "Home");
			}
			#region Check Bien
			Bien bien = await BienService.SelectBienByIdWithLocations(idbien);
            if(bien == null)
            {
                ViewData["erreur"] = "Veuillez selectionner un bien";
                return await Location();
            }
			#endregion

			#region Check Client
			Client? client = await ClientService.GetClientByIdAsync(idclient)!;
            if(client == null)
            {
                ViewData["erreur"] = "Veuillez selectionner un client";
                return await Location();
            }
            #endregion

            #region Create Location
            Location location = new()
            {
                Idclient = client.Idclient,
                Idbien = bien.Idbien
            };
            try
            {
				int IntDuree = int.Parse(duree);
                location.Duree = IntDuree;
			}
            catch { 
                ViewData["erreur"] = "La durée doit être un chiffre"; 
                return await Location(); }
            try
            {
                DateOnly DateDebut = DateOnly.Parse(datedebut);
                location.Datedebut = DateDebut;
            }
            catch { 
                ViewData["erreur"] = "Date/Format date invalide"; 
                return await Location();
            }

            try
            {
                IEnumerable<Location> Locations = await LocationService.SelectByIdBien(bien.Idbien);
                bool CheckValidite = await BienService.CheckValidite(Locations, location);
                if(CheckValidite)
                {
                    await LocationService.CreateLocation(location);
                }
            }
            catch(Exception e) { ViewData["erreur"] = e.Message;return await Location(); }
            #endregion

            ViewData["success"] = "Location ajouté";
			return await Location();
        }

        public async Task<IActionResult> Chiffre()
        {
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();
            Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> ChiffreParMois = Utils.ChiffreParMois(locations);
            ViewData["ChiffreParMois"] = ChiffreParMois;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Chiffre(DateOnly debut,DateOnly fin)
        {
            if (ContextAccessor.HttpContext!.Session.GetString("id") == null || ContextAccessor.HttpContext!.Session.GetString("id")!.Contains("A00"))
            {
                return RedirectToAction("Index", "Home");
            }
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();
            Dictionary<int, Dictionary<int, Dictionary<string, decimal>>> ChiffreParMois = Utils.ChiffreParMois(locations);
            ChiffreParMois = Utils.GainCommissionFiltreMois(ChiffreParMois, debut, fin);
            ViewData["ChiffreParMois"] = ChiffreParMois;
            return View();
        }


        public async Task<IActionResult> Liste()
        {
            IEnumerable<Location> locations = await LocationService.SelectAllAsync();
            ViewData["ListeLocation"] = locations;
            return View("ListeLocation");
        }


        public async Task<IActionResult> Details()
        {
            string idlocation = ContextAccessor.HttpContext!.Request.Path.Value!.Split("/")[3];
            Location details = await LocationService.SelectByIdLocation(idlocation);
            ViewData["details"] = details;
            return View("DetailLocation");
        }
    }
}
