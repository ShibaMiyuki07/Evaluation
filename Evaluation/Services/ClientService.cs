using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class ClientService(EvaluationsContext evaluationsContext) : IClientService
    {
        private readonly EvaluationsContext _context = evaluationsContext;

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

        public async Task<IEnumerable<Client>> GetAllClient()
        {
            return await _context.Clients.Where(c => c.Numeroclient == null).ToListAsync();
        }

        public async Task<string> CreateClientAsync(string numero)
        {
            Client cl = new()
            {
                Numeroclient = numero,
            };
            await _context.AddAsync(cl);
            await _context.SaveChangesAsync();
            return cl.Idclient;
        }

        public async Task<string> CreateProprietaireAsync(string email)
        {
            Client cl = new()
            {
                Emailclient = email,
            };
            await _context.AddAsync(cl);
            await _context.SaveChangesAsync();
            return cl.Idclient;
        }
    }
}
