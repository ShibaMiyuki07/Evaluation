using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class LocationService(EvaluationsContext evaluationsContext) : ILocationService
    {
        private readonly EvaluationsContext EvaluationsContext = evaluationsContext;

        public async Task<IEnumerable<Location>> SelectAllAsync()
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c!.IdtypebienNavigation).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectWithFiltre(DateOnly debut,Client proprietaire)
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c.IdtypebienNavigation).Where(c => c.Datedebut >= debut && c.IdbienNavigation!.Idproprietaire == proprietaire.Idclient).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SerlectByIdAndDebut(Client client,DateOnly debut)
        {
            return await EvaluationsContext.Locations.Where(c => c.Idclient == client.Idclient && c.Datedebut >= debut).ToListAsync();
        }
    }
}
