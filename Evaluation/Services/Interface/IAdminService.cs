using EvaluationClasse;

namespace Evaluation.Services.Interface
{
    public interface IAdminService
    {
        public Task<Admin> GetAdminAsync(Admin admin);
        public Task DeleteAll();
    }
}
