﻿using Evaluation.Models;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.Controllers
{
    public class ProprietaireController(IBienService bienService, IHttpContextAccessor httpContextAccessor, IClientService clientService, ILocationService location) : Controller
    {
        private readonly IBienService BienService = bienService;
        private readonly IHttpContextAccessor HttpContextAccessor = httpContextAccessor;
        private readonly IClientService ClientService = clientService;
        private readonly ILocationService LocationService = location;

        public async Task<IActionResult> Index()
        {
            if(HttpContextAccessor.HttpContext!.Session.GetString("id") == null) return RedirectToAction("Index","Home");
            string t = HttpContextAccessor.HttpContext!.Session.GetString("id")!;
            Client? proprietaire = await ClientService.GetClientByIdAsync(t)!;
			IEnumerable<Bien> liste_bien = await BienService.SelectBienByProprietaireAsync(proprietaire!);
            ViewData["liste_bien"] = liste_bien;

            Dictionary<string, DateOnly> listeDispo = await BienService.AllBienToDictionnary(proprietaire!);
            ViewData["ListeDispo"] = listeDispo;
            return View();
        }


        public async Task<IActionResult> ChiffreAffaire()
        {
            return await Task.Run(IActionResult() =>
            {
                if (HttpContextAccessor.HttpContext!.Session.GetString("id") == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                ViewData["total"] = decimal.Parse("0,0");
                return View("Chiffre");
            });
        }

        [HttpPost]
        public async Task<IActionResult> ChiffreAffaire(DateOnly debut,DateOnly fin)
        {
			if (HttpContextAccessor.HttpContext!.Session.GetString("id") == null) return RedirectToAction("Index", "Home");
			string t = HttpContextAccessor.HttpContext!.Session.GetString("id")!;
			Client? proprietaire = await ClientService.GetClientByIdAsync(t)!;
            IEnumerable<Location> locations = await LocationService.SelectAllByIdProprietaire(proprietaire!);
            decimal total = decimal.Round(Utils.CalculChiffreAffaire(locations, debut, fin),2);
            ViewData["total"] = total;
            return View("Chiffre");
        }
    }
}
