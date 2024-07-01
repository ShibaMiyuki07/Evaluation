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

		public async Task<IEnumerable<Bien>> SelectAllAsync()
		{
			return await EvaluationsContext.Biens.ToListAsync();
		}

		public async Task<Bien> SelectBienByIdWithLocations(string idbien)
		{
			return await EvaluationsContext.Biens.Include(c => c.Locations).Where(c => c.Idbien == idbien).FirstOrDefaultAsync();
		}

		public async Task<bool> CheckValidite(Bien bien,Location location)
		{
			bool retour = true;
			foreach(Location l in  bien.Locations)
			{
				DateOnly locationFin = l.Datedebut!.Value.AddMonths((int)l.Duree!);
				if(location.Datedebut == l.Datedebut)
				{
					throw new Exception("Date déjà prise");
				}
				if(location.Datedebut>l.Datedebut && l.Datedebut<locationFin)
				{
					throw new Exception("Le bien est en cours de location");
				}
				if(location.Datedebut<l.Datedebut && (location.Datedebut.Value!.AddMonths((int)location.Duree) < locationFin || location.Datedebut.Value!.AddMonths((int)location.Duree) >= locationFin))
				{
					throw new Exception("Il y a déjà une reservation entre cet intervalle");
				}
			}
			return retour;
		}


		public async Task<Dictionary<string,DateOnly>> AllBienToDictionnary(Client client)
		{
			Dictionary<string, DateOnly> retour = [];
			IEnumerable<Bien> listes = await SelectBienProprietaireWithLocation(client);
			foreach (Bien b in listes)
			{
                //Check si date debut <= maintenant
                b.Locations = b.Locations.OrderBy(c => c.Datedebut).ToList();
                Location last = b.Locations.Last();
				if(last.Datedebut <= DateOnly.FromDateTime(DateTime.Now))
				{
                    DateOnly dispo = last.Datedebut.Value!.AddMonths((int)last.Duree!);
                    retour.Add(b.Idbien, dispo);
                }
				else
				{
                    DateOnly dispo = DateOnly.FromDateTime(DateTime.Now);
                    retour.Add(b.Idbien, dispo);
                }
            }
			return retour;
		}

		public async Task CreateDataFromCSV(IEnumerable<Evaluaton.Models.Csv.Bien> listes)
		{
			List<Bien> lists = [];
			foreach(var l in listes)
			{
				Typebien typebien = await typeBienService.GetTypebienByNameAsync(l.Type);
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
