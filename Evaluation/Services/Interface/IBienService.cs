﻿using EvaluationClasse;

namespace Evaluation.Services.Interface
{
	public interface IBienService
	{
		public Task<IEnumerable<Bien>> SelectBienByProprietaireAsync(Client client);
		public Task<IEnumerable<Bien>> SelectBienProprietaireWithLocation(Client client);
		public Task CreateDataFromCSV(IEnumerable<Evaluaton.Models.Csv.Bien> listes);
		public Task<Dictionary<string, DateOnly>> AllBienToDictionnary(Client client);

	}
}
