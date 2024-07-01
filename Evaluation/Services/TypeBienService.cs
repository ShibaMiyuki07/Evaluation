using Evaluation.Context;
using Evaluation.Models.Csv;
using Evaluation.Services.Interface;
using EvaluationClasse;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Services
{
    public class TypeBienService(EvaluationsContext evaluationsContext) : ITypeBienService
    {
        private EvaluationsContext _context = evaluationsContext;

        public async Task<Typebien> GetTypebienByNameAsync(string name)
        {
            return await _context.Typebiens.Where(t => t.Type.Contains(name)).FirstOrDefaultAsync()!;
        }

        public async Task CreateTypeBienFromCsv(IEnumerable<Commissions> Commissions)
        {
            foreach (var commission in Commissions)
            {
                Typebien typebien = new() { 
                    Type = commission.Type,
                    Commission = decimal.Parse(commission.Commission),
                };
                await _context.AddAsync(typebien);
                await _context.SaveChangesAsync();
            }
        }
    }
}
