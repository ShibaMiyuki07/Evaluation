using EvaluationClasse;

namespace Evaluation.Services.Interface
{
    public interface IClientService
    {
        public Task<Client> GetClientByEmail(Admin admin);
        public Task<Client> GetClientByNumero(Admin admin);
        public Task<Client?> GetClientByIdAsync(string idclient);
        public Task<string> CreateClientAsync(string numero);
        public Task<IEnumerable<Client>> GetAllClient();
        public Task<string> CreateProprietaireAsync(string email);
    }
}
