using Microsoft.EntityFrameworkCore;

namespace SuperhumanAPI.Data
{
    public class MutantContext : DbContext
    {
        public MutantContext(DbContextOptions<MutantContext> options)
            : base(options)
        {
        }
        public DbSet<SuperhumanAPI.Models.Mutant> Mutants { get; set; }
    }
}
