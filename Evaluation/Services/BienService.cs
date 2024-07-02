using EFCore.BulkExtensions;
using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class BienService(EvaluationsContext evaluationsContext,
		ITypeBienService typeBienService,
		IClientService clientService) : IBienService
	{
		private readonly EvaluationsContext EvaluationsContext = evaluationsContext;
		private readonly ITypeBienService typeBienService = typeBienService;
		private readonly IClientService clientService = clientService;

        public async Task<IEnumerable<Bien>> SelectBienByProprietaireAsync(Client client)
		{
			return await EvaluationsContext.Biens.Include(c => c.Locations).Where(c => c.Idproprietaire == client.Idclient).ToListAsync();
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
			return await EvaluationsContext.Biens.Include(c => c.Locations).Include(c => c.IdtypebienNavigation).Where(c => c.Idbien == idbien).FirstOrDefaultAsync();
		}

		public async Task<bool> CheckValidite(IEnumerable<Location> Locations,Location location)
		{
			bool retour = true;
			foreach (var l in Locations)
			{
				foreach (var lpm in l.Locationparmois)
				{
					for (int i = 0; i < location.Duree; i++)
					{
						if (location.Datedebut!.Value.Year == lpm.Annee)
						{
							if (location.Datedebut.Value.Month == lpm.Mois)
							{
								throw new Exception("Il y a une date déjà prise");
							}
						}
					}
				}
			}
			return retour;
		}


		public async Task<Dictionary<string,DateOnly>> AllBienToDictionnary(Client client)
		{
			Dictionary<string, DateOnly> retour = [];
			IEnumerable<Bien> listes = await SelectBienProprietaireWithLocation(client);
			DateOnly Maintenant = DateOnly.FromDateTime(DateTime.Now);

            foreach (Bien bien in listes)
			{
				foreach(Location location in bien.Locations)
				{
					DateOnly DateDebutLocation = new(location.Datedebut!.Value.Year, location.Datedebut!.Value.Month, 1);
					if(DateDebutLocation <= Maintenant && DateDebutLocation.AddMonths((int)location.Duree!) > Maintenant)
					{
                            if (retour.ContainsKey(bien.Idbien))
                            {
								if (DateDebutLocation > Maintenant && DateDebutLocation >= retour[bien.Idbien])
								{
									retour.Remove(bien.Idbien);
									if(DateDebutLocation.DayNumber - retour[bien.Idbien].DayNumber >= 30)
									{
										retour.Add(bien.Idbien, Maintenant);
									}
									else retour.Add(bien.Idbien, DateDebutLocation.AddMonths((int)location.Duree!));

                            }
							}
							else retour.Add(bien.Idbien, DateDebutLocation.AddMonths((int)location.Duree!));
                    }
					else if(DateDebutLocation > Maintenant && DateDebutLocation > Maintenant.AddMonths(1))
					{
                        retour.Remove(bien.Idbien);
                        retour.Add(bien.Idbien, Maintenant);
                    }
					else
					{
                        retour.Remove(bien.Idbien);
                        retour.Add(bien.Idbien, Maintenant);
                    }
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
				Bien nouveau = new() 
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
