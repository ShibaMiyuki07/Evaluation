﻿using EvaluationClasse;

namespace Evaluation.Services.Interface
{
	public interface IBienService
	{
		public Task<IEnumerable<Bien>> SelectBienByProprietaireAsync(Client client);
	}
}
