using EvaluationClasse;

namespace Evaluation.Services.Interface
{
    public interface ITypeBienService
    {
        public Task<Typebien> GetTypebienByNameAsync(string name);
    }
}
