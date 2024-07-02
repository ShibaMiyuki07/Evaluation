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

        public async Task DeleteAll()
        {
            _evaluationContext.Database.ExecuteSqlRaw("alter sequence idadmin minvalue 0 restart with 1 ;");
            _evaluationContext.Database.ExecuteSqlRaw("alter sequence idbien minvalue 0 restart with 1 ;");
            _evaluationContext.Database.ExecuteSqlRaw("alter sequence idclient minvalue 0 restart with 1 ;");
            _evaluationContext.Database.ExecuteSqlRaw("alter sequence idlocation minvalue 0 restart with 1 ;");
			_evaluationContext.Database.ExecuteSqlRaw("alter sequence idlocationparmois minvalue 0 restart with 1 ;");
			_evaluationContext.Database.ExecuteSqlRaw("alter sequence idtypebien minvalue 0 restart with 1 ;");


			_evaluationContext.Database.ExecuteSqlRaw("delete from paye;");
            _evaluationContext.Database.ExecuteSqlRaw("delete from locationparmois;");
            _evaluationContext.Database.ExecuteSqlRaw("delete from location;");
            _evaluationContext.Database.ExecuteSqlRaw("delete from biens;");
			_evaluationContext.Database.ExecuteSqlRaw("delete from typebien;");
			_evaluationContext.Database.ExecuteSqlRaw("delete from client;");
            await _evaluationContext.SaveChangesAsync();
        }

    }
}
