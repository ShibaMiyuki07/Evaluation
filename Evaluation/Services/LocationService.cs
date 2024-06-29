using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class LocationService : ILocationService
    {
        private readonly EvaluationsContext EvaluationsContext;

        public LocationService(EvaluationsContext evaluationsContext)
        {
            EvaluationsContext = evaluationsContext;
        }
        public async Task<IEnumerable<Location>> SelectAllAsync()
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c!.IdtypebienNavigation).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectWithFiltre(DateOnly debut,Client proprietaire)
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c.IdtypebienNavigation).Where(c => c.Datedebut >= debut && c.IdbienNavigation!.Idproprietaire == proprietaire.Idclient).ToListAsync();
        }

        //public async Task<decimal> ChiffreAffaire(DateOnly debut,DateOnly fin)
        //{
        //    decimal retour = 0;
        //    IEnumerable<Location> locations = await this.SelectWithFiltre(debut);
        //    foreach (Location location in locations)
        //    {
        //        decimal a_ajouter = 0;
        //        int duree = 0;
        //        for(int i = location.Datedebut!.Value.Month; i <= fin.Month; i++)
        //        {
        //            i++;
        //        }
        //        a_ajouter =(decimal) (location.IdbienNavigation!.Loyer!-Commission(location))*duree;

        //        retour += a_ajouter;
        //    }
        //    return retour;
        //}

        //public decimal Commission(Location location)
        //{
        //    return (decimal)(location.IdbienNavigation!.Loyer! * location.IdbienNavigation.IdtypebienNavigation!.Commission!) / 100;
        //}
    }
}
