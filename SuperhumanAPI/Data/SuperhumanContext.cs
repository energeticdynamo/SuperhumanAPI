using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Data
{
    //This is an example of dependency injection in the api.
    public class SuperhumanContext : DbContext
    {
        public SuperhumanContext(DbContextOptions<SuperhumanContext> options)
            : base(options)
        {
        }

        public DbSet<SuperhumanDTO> Superhumans { get; set; }
    }
}