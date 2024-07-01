using EFCore.BulkExtensions;
using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
	public class BienService(EvaluationsContext evaluationsContext,ITypeBienService typeBienService,IClientService clientService) : IBienService
	{
		private readonly EvaluationsContext EvaluationsContext = evaluationsContext;
		private readonly ITypeBienService typeBienService = typeBienService;
		private readonly IClientService clientService = clientService;

        public async Task<IEnumerable<Bien>> SelectBienByProprietaireAsync(Client client)
		{
			return await EvaluationsContext.Biens.Where(c => c.Idproprietaire == client.Idclient).ToListAsync();
		}

		public async Task<IEnumerable<Bien>> SelectBienProprietaireWithLocation(Client client)
		{
			return await EvaluationsContext.Biens.Include(c => c.Locations).Include(c => c.IdtypebienNavigation).Where(c => c.Idproprietaire == client.Idclient).ToListAsync() ;
		}


		public async Task<Dictionary<string,DateOnly>> AllBienToDictionnary(Client client)
		{
			Dictionary<string, DateOnly> retour = [];
			IEnumerable<Bien> listes = await SelectBienProprietaireWithLocation(client);
			foreach (Bien b in listes)
			{
				b.Locations = b.Locations.OrderBy(c => c.Datedebut).ToList();
				Location last = b.Locations.Last();
				DateOnly dispo = last.Datedebut.Value!.AddMonths((int)last.Duree!);
				retour.Add(b.Idbien, dispo);
			}
			return retour;
		}

		public async Task CreateDataFromCSV(IEnumerable<Evaluaton.Models.Csv.Bien> listes)
		{
			List<Bien> lists = [];
			foreach(var l in listes)
			{
				Typebien typebien = await typeBienService.GetTypebienByNameAsync(l.Type.ToLower());
				Client client = await clientService.GetClientByNumero(new Admin() { Login = l.Proprietaire});
				if(client == null)
				{
					client = new();
					client!.Idclient = await clientService.CreateClientAsync(l.Proprietaire);
				}
				Bien nouveau = new Bien() 
				{ 
					Idbien = l.Reference,
					Nombien = l.Nom,
					Description = l.Description,
					Region = l.Region,
					Idtypebien = typebien.Idtypebien,
					Idproprietaire = client.Idclient,
					Loyer = l.Loyer,
				};
				lists.Add(nouveau);
			}
			await EvaluationsContext.BulkInsertAsync(lists);
			await EvaluationsContext.SaveChangesAsync();
		}
	}
}
