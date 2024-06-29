﻿using EvaluationClasse;

namespace Evaluation.Services.Interface
{
    public interface IClientService
    {
        public Task<Client> GetClientByEmail(Admin admin);
        public Task<Client> GetClientByNumero(Admin admin);
    }
}