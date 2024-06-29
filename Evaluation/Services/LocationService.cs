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
    }
}
