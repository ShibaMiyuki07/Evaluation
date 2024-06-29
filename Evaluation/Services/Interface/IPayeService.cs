using EvaluationClasse;

namespace Evaluation.Services.Interface
{
    public interface IPayeService
    {
        public Task<IEnumerable<Paye>> SelectByClientAsync(Client client);
    }
}
