using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class PayeService(EvaluationsContext evaluationsContext) : IPayeService
    {
        private readonly EvaluationsContext evaluationsContext = evaluationsContext;

        public async Task<IEnumerable<Paye>> SelectByClientAsync(Client client)
        {
            return await evaluationsContext.Payes.Include(c => c.IdlocationNavigation).Where(c => c.IdlocationNavigation!.Idclient == client.Idclient).ToListAsync();
        }
    }
}
