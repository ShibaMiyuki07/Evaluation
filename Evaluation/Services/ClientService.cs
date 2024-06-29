using Evaluation.Context;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class ClientService
    {
        private readonly EvaluationsContext _context;
        public ClientService(EvaluationsContext evaluationsContext) { _context = evaluationsContext; }

        public async Task<Client> GetClientByEmail(Admin admin)
        {
            Client? cl = await _context.Clients.Where(c => c.Emailclient == admin.Login).FirstOrDefaultAsync()!;
            return cl!;
        }

        public async Task<Client> GetClientByNumero(Admin admin)
        {
            Client? cl = await _context.Clients.Where(c => c.Numeroclient == admin.Login).FirstOrDefaultAsync()!;
            return cl!;
        }
    }
}
