using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Data;

public class MutantContext : DbContext
{
    public MutantContext(DbContextOptions<MutantContext> options) : base(options) { }
    public DbSet<Mutant> Mutants { get; set; } = null!;
}