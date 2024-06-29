using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
	public class BienService : IBienService
	{
		private readonly EvaluationsContext EvaluationsContext;

		public BienService(EvaluationsContext evaluationsContext)
		{
			EvaluationsContext = evaluationsContext;
		}

		public async Task<IEnumerable<Bien>> SelectBienByProprietaireAsync(Client client)
		{
			return await EvaluationsContext.Biens.Where(c => c.Idproprietaire == client.Idclient).ToListAsync();
		}

		public async Task<IEnumerable<Bien>> SelectBienProprietaireWithLocation(Client client)
		{
			return await EvaluationsContext.Biens.Include(c => c.Locations).Include(c => c.Idtypebien).Where(c => c.Idproprietaire == client.Idclient).ToListAsync() ;
		}
	}
}
