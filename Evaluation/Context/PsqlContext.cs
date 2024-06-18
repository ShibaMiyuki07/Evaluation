using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Evaluation.Context
{
    public class PsqlContext : DbContext
    {
        public PsqlContext(DbContextOptions<PsqlContext> options) : base(options)
        { }
    }
}
