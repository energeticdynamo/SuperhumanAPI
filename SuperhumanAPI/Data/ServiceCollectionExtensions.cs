using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuperhumanAPI.Repositories;

namespace SuperhumanAPI.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SuperhumanContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<TeamContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<MutantContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ITeams, TeamsRepository>();

            // Add more contexts here as needed

            return services;
        }
    }
}
