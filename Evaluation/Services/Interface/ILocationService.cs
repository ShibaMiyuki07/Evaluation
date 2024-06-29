using EvaluationClasse;

namespace Evaluation.Services.Interface
{
    public interface ILocationService
    {
        public Task<IEnumerable<Location>> SelectAllAsync();

        public Task<IEnumerable<Location>> SelectWithFiltre(DateOnly debut, Client proprietaire);

        public Task<IEnumerable<Location>> SerlectByIdAndDebut(Client client, DateOnly debut);


    }
}
