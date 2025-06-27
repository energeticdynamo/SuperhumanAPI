using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Data
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions<TeamContext> options)
            : base(options)
        {
        }
        public DbSet<Teams> Teams { get; set; }
    }
}
