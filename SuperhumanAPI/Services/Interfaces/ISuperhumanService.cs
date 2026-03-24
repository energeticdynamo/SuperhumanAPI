using SuperhumanAPI.Models;

namespace SuperhumanAPI.Services.Interfaces
{
    public interface ISuperhumanService
    {
        Task<List<string>> GetTopTierHeroNamesAsync();
        Task AddHeroToTeamAsync(Superhuman newHero);
    }
}
