using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class ClientService : IClientService
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

        public async Task<Client?> GetClientByIdAsync(string idclient)
        {
            return await _context.Clients.Where(c => c.Idclient == idclient).FirstOrDefaultAsync()!;
        }
    }
}
