using EFCore.BulkExtensions;
using Evaluation.Context;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class LocationService(EvaluationsContext evaluationsContext,IClientService clientService,IBienService bienService) : ILocationService
    {
        private readonly EvaluationsContext EvaluationsContext = evaluationsContext;
        private readonly IClientService clientService = clientService;
        private readonly IBienService BienService = bienService;

        public async Task<IEnumerable<Location>> SelectAllAsync()
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c!.IdtypebienNavigation).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectByDateDebut(DateOnly debut)
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c!.IdtypebienNavigation).Where(c => (c.Datedebut >= debut || (c.Datedebut.Value.AddMonths((int)c.Duree))>debut)).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SelectWithFiltre(DateOnly debut,Client proprietaire)
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).ThenInclude(c => c.IdtypebienNavigation).Where(c => c.Datedebut >= debut && c.IdbienNavigation!.Idproprietaire == proprietaire.Idclient).ToListAsync();
        }

        public async Task<IEnumerable<Location>> SerlectByIdAndDebut(Client client,DateOnly debut)
        {
            return await EvaluationsContext.Locations.Include(c => c.IdbienNavigation).Where(c => c.Idclient == client.Idclient && (c.Datedebut >= debut || c.Datedebut!.Value.AddMonths((int)c.Duree!) >= debut)).ToListAsync();
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
                Location nouveau = new()
                {
                    Idbien = location.Reference,
                    Datedebut = location.DateDebut,
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
			for (int i = 0; i < nouveau.Duree; i++)
			{
				Locationparmoi locationparmoi = new()
				{
					Idlocation = nouveau.Idlocation,
					Mois = nouveau.Datedebut!.Value!.Month,
					Annee = nouveau.Datedebut.Value!.Year,
					Montant = bien.Loyer
				};

				await EvaluationsContext.AddAsync(locationparmoi);
				await EvaluationsContext.SaveChangesAsync();
				nouveau.Datedebut = nouveau.Datedebut.Value!.AddMonths(1);
			}
		}
    }
}
