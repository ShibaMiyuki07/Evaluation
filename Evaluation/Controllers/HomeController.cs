using Evaluation.Log.Interface;
using Evaluation.Models;
using Evaluation.Models.Csv;
using Evaluation.Services.Interface;
using Evaluation.Services.Utile;
using EvaluationClasse;
using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Evaluation.Controllers
{
    public class HomeController(ILoggerManager logger,
        IRazorViewRenderer razorViewRenderer,
        IHttpContextAccessor httpContextAccessor,
        IClientService clientService,
        IAdminService adminService,
        IBienService bienService,
        ILocationService location,
        ITypeBienService typeBienService) : Controller
    {
        private readonly IRazorViewRenderer razorViewRenderer = razorViewRenderer;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        private readonly IClientService ClientService = clientService;
        private readonly IAdminService AdminService = adminService;
        private readonly ILoggerManager loggerManager = logger;
        private readonly IBienService bienService = bienService;
        private readonly ILocationService locationService = location;
        private readonly ITypeBienService TypeBienService = typeBienService;

        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                /*Test creation pdf
                //IronPdf.License.LicenseKey = "IRONSUITE.MAMIHERIMANITRA.RAKOTOARISOA.PULSE.MG.28235-FFF220765A-H3A7R-D6L2K33YJDVE-F2ZFRCHMRBIB-ZPLIV2QWNY55-ZJF7JQMHK7IA-OAKXC3THNMTN-QEDLJYQOBOZA-VEKXDK-TT5FXEF4IBKNEA-DEPLOYMENT.TRIAL-TWDSRD.TRIAL.EXPIRES.24.JUL.2024";
                //PdfService pdf = new(razorViewRenderer);

                //PdfDocument pdfdoc = await pdf.CreatePdf<Constante>("Views/Home/Test.cshtml", null);
                //Response.Headers.Add("Content-Disposition", "inline");

                //return File(pdfdoc.BinaryData, "application/pdf","test.pdf");*/
                return View();
            });
            /*
            //IEnumerable<Joueur> csv = new CsvImporterService<Joueur>().Import("..\\evaluation_03_2024_donnee - joueurs.csv");
            //ViewData["Joueurs"] = csv;

            //Test file return from controller
            //Response.Headers.Add("Content-Disposition", "inline");
            //return File(pdf.BinaryData, "application/pdf", "viewToPdfMVCCore.pdf");
            */
        }

        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            IActionResult retour = null;
            if (admin.Login != null)
            {
                if (admin.Mdp == null || admin.Mdp == string.Empty)
                {
                    #region Client
                    if (Utils.CheckEmail(admin.Login!))
                    {
                        try
                        {
                            retour = await HomeToClient(admin);
                        }
                        catch (Exception ex) { }
                    }
                    #endregion

                    #region Proprietaire
                    if (Utils.CheckNumero(admin.Login!))
                    {
                        try
                        {
                            retour = await HomeToProprietaire(admin);
                        }
                        catch(Exception ex) { }
                    }
                    #endregion
                }

                if(retour!= null)
                {
                    return retour;
                }
                #region Admin
                admin = await AdminService.GetAdminAsync(admin);
                if (admin == null || admin.Idadmin == string.Empty || admin.Idadmin == null)
                {
                    ViewData["erreur"] = "Identifiant inexistant";
                    return View("Index");
                }
                else
                {
                    _contextAccessor.HttpContext!.Session.SetString("id", admin.Idadmin);
                    return RedirectToAction("Index", "Admin");
                }
                #endregion
            }
            else
            {
                ViewData["erreur"] = "Entrez des données valides";
                return View("Index");
            }
        }

        #region HomeToProprietaire
        public async Task<IActionResult> HomeToProprietaire(Admin admin)
        {
            Client client = await ClientService.GetClientByNumero(admin);
           try
            {
                _contextAccessor.HttpContext!.Session.SetString("id", client.Idclient);
            }
            catch { throw; }
            return RedirectToAction("Index", "Proprietaire");
        }
        #endregion

        #region HomeToClient
        public async Task<IActionResult> HomeToClient(Admin admin)
        {
            Client client = await ClientService.GetClientByEmail(admin);
            try
            {
                _contextAccessor.HttpContext!.Session.SetString("id", client.Idclient);
            }
            catch { throw; }
            return RedirectToAction("Index", "Client");
        }
        #endregion

        /*
        //Exemple pour l'insertion des données
        //[HttpPost]
        //public IActionResult Index(User m)
        //{
        //    if(m.isValid())
        //{
        //  Console.WriteLine(m.test);
        //}
        //    return View();
        //}


        //Exemple retrieving file
        //[HttpPost]
        //public IActionResult Index(IFormFile file)
        //{
        //    Console.WriteLine(file.FileName);
        //    return View();
        //}
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.Run(() =>
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            });
        }


        public async Task<IActionResult> Logout()
        {
            return await Task.Run(IActionResult() =>
            {
                _contextAccessor.HttpContext!.Session.Remove("id");
                return RedirectToAction("Index", "Home");
            });
        }


        public async Task<IActionResult> Import()
        {
            return await Task.Run(IActionResult() =>
            {
                return View();
            });
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if(file == null)
            {
                ViewData["erreur"] = "Fichier invalide"; 
                return View();
            }
            if(file.FileName.ToLower().Contains("location"))
            {
                IEnumerable<Evaluaton.Models.Csv.Location> locations = await new CsvImporterService<Evaluaton.Models.Csv.Location>(loggerManager).ImportFromIFormFile(file);
                await locationService.CreateDataFromCSV(locations);
                ViewData["success"] = "Toutes les locations ont été ajouté avec succés";
            }
            if(file.FileName.ToLower().Contains("bien"))
            {
                IEnumerable<Evaluaton.Models.Csv.Bien> biens = await new CsvImporterService<Evaluaton.Models.Csv.Bien>(loggerManager).ImportFromIFormFile(file);
                await bienService.CreateDataFromCSV(biens);
                ViewData["success"] = "Tout les biens ont été ajouté avec succés";
            }
            if(file.FileName.ToLower().Contains("commission"))
            {
                IEnumerable<Commissions> Commissions = await new CsvImporterService<Commissions>(loggerManager).ImportFromIFormFile(file);
                await TypeBienService.CreateTypeBienFromCsv(Commissions);
                ViewData["success"] = "Tout les types de bien ont été ajouté avec succès";

			}
            return View();
        }
    }
}
