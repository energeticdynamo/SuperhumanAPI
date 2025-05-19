using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Data
{
    public class SuperhumanContext : DbContext
    {
        public SuperhumanContext(DbContextOptions<SuperhumanContext> options)
            : base(options)
        {
        }

        public DbSet<Superhuman> Superhumans { get; set; }
    }
}