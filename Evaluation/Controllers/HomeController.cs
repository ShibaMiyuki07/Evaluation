using Evaluation.Log.Interface;
using Evaluation.Models;
using Evaluation.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Evaluation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerManager _logger;

        public HomeController(ILoggerManager logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInfo("Ouverture de l'index");
            //IEnumerable<Joueur> csv = new CsvImporterService<Joueur>().Import("..\\evaluation_03_2024_donnee - joueurs.csv");
            //ViewData["Joueurs"] = csv;
            return View();
        }


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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
