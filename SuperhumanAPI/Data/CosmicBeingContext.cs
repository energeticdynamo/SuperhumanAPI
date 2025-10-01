using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Data
{
    public class CosmicBeingContext : DbContext
    {
        public CosmicBeingContext(DbContextOptions<CosmicBeingContext> options) : base(options) { }
        public DbSet<CosmicBeing> CosmicBeings { get; set; } = null!;
    }
}
