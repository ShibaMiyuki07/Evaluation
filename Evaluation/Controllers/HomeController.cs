using Evaluation.Log.Interface;
using Evaluation.Models;
using Evaluation.Services;
using Evaluation.Services.Interface;
using EvaluationClasse;
using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Evaluation.Controllers
{
    public class HomeController(ILoggerManager logger,IRazorViewRenderer razorViewRenderer,IHttpContextAccessor httpContextAccessor,IClientService clientService,IAdminService adminService) : Controller
    {
        private readonly ILoggerManager _logger = logger;
        private readonly IRazorViewRenderer razorViewRenderer = razorViewRenderer;
        private readonly IHttpContextAccessor _contextAccessor = httpContextAccessor;
        private readonly IClientService ClientService = clientService;
        private readonly IAdminService AdminService = adminService;

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
            if(admin.Login != null)
            {
                if (admin.Mdp == null || admin.Mdp == string.Empty)
                {
                    #region Client
                    if (Utils.CheckEmail(admin.Login!))
                    {
                        await HomeToClient(admin);
                    }
                    #endregion

                    #region Proprietaire
                    if (Utils.CheckNumero(admin.Login!))
                    {
                        await HomeToProprietaire(admin);
                    }
                    #endregion
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
            _contextAccessor.HttpContext!.Session.SetString("id", client.Idclient);
            return RedirectToAction("Index", "Proprietaire");
        }
        #endregion

        #region HomeToClient
        public async Task<IActionResult> HomeToClient(Admin admin)
        {
            Client client = await ClientService.GetClientByEmail(admin);
            _contextAccessor.HttpContext!.Session.SetString("id", client.Idclient);
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
    }
}
