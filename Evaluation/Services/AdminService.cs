using Evaluation.Context;
using Evaluation.Log.Interface;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;


namespace Evaluation.Services
{
    public class AdminService(EvaluationsContext evaluationsContext,ILoggerManager loggerManager) : IAdminService
    {
        private readonly EvaluationsContext _evaluationContext = evaluationsContext;
        private readonly ILoggerManager LoggerManager = loggerManager;

        public async Task<Admin> GetAdminAsync(Admin admin)
        {
            return await _evaluationContext.Admins.Where(a => a.Login ==  admin.Login && a.Mdp == admin.Mdp).FirstOrDefaultAsync()!;
        }

        public async Task DeleteAll()
        {
            LoggerManager.LogWarn("Starting to Delete all data from database");
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
            LoggerManager.LogInfo("End of deletion");
        }

    }
}
