using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;


namespace Evaluation.Services
{
    public class AdminService(EvaluationsContext evaluationsContext) : IAdminService
    {
        private readonly EvaluationsContext _evaluationContext = evaluationsContext;

        public async Task<Admin> GetAdminAsync(Admin admin)
        {
            return await _evaluationContext.Admins.Where(a => a.Login ==  admin.Login && a.Mdp == admin.Mdp).FirstOrDefaultAsync()!;
        }
    }
}
