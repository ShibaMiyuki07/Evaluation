using EFCore.BulkExtensions;
using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Evaluation.Services
{
    public class LocationService(EvaluationsContext evaluationsContext,IClientService clientService,IBienService bienService) : ILocationService
    {
        private readonly EvaluationsContext EvaluationsContext = evaluationsContext;
        private readonly IClientService clientService = clientService;
        private readonly IBienService BienService = bienService;

        public async Task<IEnumerable<Location>> SelectAllAsync()
        {
            return await EvaluationsContext.Locations.Include(c => c.Locationparmois).Include(c => c.IdbienNavigation).ThenInclude(c => c!.IdtypebienNavigation).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectByDateDebut(DateOnly debut)
        {
            return await EvaluationsContext.Locations.Include(c => c.Locationparmois).Include(c => c.IdbienNavigation).ThenInclude(c => c!.IdtypebienNavigation).Where(c => (c.Datedebut >= debut || (c.Datedebut.Value.AddMonths((int)c.Duree))>debut)).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectWithFiltre(DateOnly debut,Client proprietaire)
        {
            return await EvaluationsContext.Locations.Include(c => c.Locationparmois).Include(c => c.IdbienNavigation).ThenInclude(c => c.IdtypebienNavigation).Where(c => c.Datedebut >= debut && c.IdbienNavigation!.Idproprietaire == proprietaire.Idclient).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectAllByIdProprietaire(Client proprietaire)
        {
            return await EvaluationsContext.Locations.Include(c => c.Locationparmois).Include(c => c.IdbienNavigation).ThenInclude(c => c.IdtypebienNavigation).Where(c => c.IdbienNavigation!.Idproprietaire == proprietaire.Idclient).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectByIdAndDebut(Client client,DateOnly debut)
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).Where(c => c.Idclient == client.Idclient && (c.Datedebut >= debut || c.Datedebut!.Value.AddMonths((int)c.Duree!) >= debut)).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectByIdBien(string idbien)
        {
            return await EvaluationsContext.Locations.Include(c => c.Locationparmois).Where(c => c.Idbien == idbien).ToListAsync();
        }


        public async Task CreateDataFromCSV(IEnumerable<Evaluaton.Models.Csv.Location> listes)
        {
            foreach(var location in listes)
            {
                Client Proprietaire = await clientService.GetClientByEmail(new Admin() { Login = location.Client });
                if(Proprietaire == null)
                {
                    Proprietaire = new();
                    Proprietaire.Idclient = await clientService.CreateProprietaireAsync(location.Client);
                }
                Bien bien = await BienService.SelectBienByIdWithLocations(location.Reference);
                string[] date = [];
                date = location.DateDebut.Split('-');
                if (date.Length == 1)
                {
                    date = location.DateDebut.Split('/');
                }
                string mois = Constante.mois.Where(x => x.Item2.ToLower() == date[1].ToLower()).Select(x => x.Item1.ToString()).FirstOrDefault()!;
                if(mois == null || mois == string.Empty) 
                { 
                    mois = Constante.mois.Where(x => x.Item1 == int.Parse(date[1])).Select(x => x.Item1.ToString()).FirstOrDefault()!;
                }
                DateOnly DateDebut = new DateOnly(int.Parse(date[2]), int.Parse(mois), int.Parse(date[0]));
                Location nouveau = new()
                {
                    Idbien = location.Reference,
                    Datedebut = DateDebut,
                    Duree = location.Duree,
                    Idclient = Proprietaire.Idclient,
                };
                await EvaluationsContext.AddAsync(nouveau);
				await EvaluationsContext.SaveChangesAsync();

                await CreateLocationMois(nouveau, bien);
			}
        }

        public async Task CreateLocation(Location location)
        {
            await EvaluationsContext.AddAsync(location);
            await EvaluationsContext.SaveChangesAsync();
            Bien bien = await BienService.SelectBienByIdWithLocations(location.Idbien);
            await CreateLocationMois(location, bien);
        }

        private async Task CreateLocationMois(Location nouveau,Bien bien)
        {
            DateOnly date = new(nouveau.Datedebut!.Value.Year,nouveau.Datedebut!.Value.Month,1);
			for (int i = 0; i < nouveau.Duree; i++)
			{
                decimal commission = decimal.Parse("0");
                if(date.Month == nouveau.Datedebut!.Value.Month && date.Year == nouveau.Datedebut!.Value.Year)
                {
                    commission = decimal.Parse("100");
                }
                else
                {
                    commission = (decimal)bien.IdtypebienNavigation!.Commission!;
                }
                Locationparmoi locationparmoi = new()
                {
                    Idlocation = nouveau.Idlocation,
                    Mois = date.Month,
                    Annee = date.Year,
                    Montant = bien.Loyer,
                    Commission = commission
				};

				await EvaluationsContext.AddAsync(locationparmoi);
				await EvaluationsContext.SaveChangesAsync();
				date = date.AddMonths(1);
			}
		}
    }
}
