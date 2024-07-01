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
    }
}
