using SuperhumanAPI.Repositories.Implementations;
using SuperhumanAPI.Repositories.Interfaces;
using SuperhumanAPI.Services.Interfaces;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
namespace SuperhumanAPI.Services.Implementations
{
    public class SuperhumanService : ISuperhumanService
    {
        private readonly ISuperhumanRepository _superhumanRepository;
        private readonly IMutantRepository _mutantRepository;

        public SuperhumanService(ISuperhumanRepository superhumanRepository, IMutantRepository mutantRepository)
        {
            _superhumanRepository = superhumanRepository;
            _mutantRepository = mutantRepository;
        }

        public async Task<List<string>> GetTopTierHeroNamesAsync()
        {
            var result = await _superhumanRepository.GetAllSuperhumansAsync(1, int.MaxValue);
            var superhumans = result.Items?.ToList() ?? new List<Superhuman>();
            return superhumans
                .Where(s => s.Ranking.HasValue && s.Ranking.Value >= 8)
                .Select(s => s.CodeName ?? s.FirstName)
                .ToList();
        }

        public async Task AddHeroToTeamAsync(Superhuman newHero)
        {
            // 1. If the hero isn't joining a team, just add them and exit
            if (newHero.TeamId == null)
            {
                await _superhumanRepository.AddSuperhumanAsync(newHero);
                return;
            }

            // 2. Get the current members
            var result = await _superhumanRepository.GetAllSuperhumansAsync(1, 100);
            var currentTeamCount = result.Items?.Count(s => s.TeamId == newHero.TeamId) ?? 0;

            // 3. The "Gatekeeper" Check
            if (currentTeamCount >= 6)
            {
                // In C#, we usually throw 'InvalidOperationException' for business rule violations
                throw new InvalidOperationException("This team has reached its maximum capacity of 6 members.");
            }

            // 4. Save to Database
            await _superhumanRepository.AddSuperhumanAsync(newHero);
        }

        public async Task AddSuperhumanSafeAsync(Superhuman newHero)
        {
            // The "Senior" way: Ask the database directly!
            bool alreadyExists = await _mutantRepository.DoesMutantExistAsync(newHero.FirstName, lastName: newHero.LastName);

            if (alreadyExists)
            {
                throw new InvalidOperationException($"{newHero.FirstName} {newHero.LastName} is already a registered Mutant.");
            }

            await _superhumanRepository.AddSuperhumanAsync(newHero);
        }
    }
}
