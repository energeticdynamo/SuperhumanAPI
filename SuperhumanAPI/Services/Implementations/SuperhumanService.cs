using SuperhumanAPI.Repositories.Implementations;
using SuperhumanAPI.Repositories.Interfaces;
using SuperhumanAPI.Services.Interfaces;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
namespace SuperhumanAPI.Services.Implementations
{
    public class SuperhumanService : ISuperhumanService
    {
        private readonly ISuperhumanRepository _superhumanRepository;
        private readonly IMutantRepository _mutantRepository;
        private readonly ITeamsRepository _teamsRepository;

        public SuperhumanService(ISuperhumanRepository superhumanRepository, IMutantRepository mutantRepository, ITeamsRepository teamsRepository)
        {
            _superhumanRepository = superhumanRepository;
            _mutantRepository = mutantRepository;
            _teamsRepository = teamsRepository;
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

        public async Task OverPoweredValidation(Superhuman newHero)
        {            
            if (newHero.Ranking == 10 && !string.IsNullOrEmpty(newHero.SecondaryPower))
            {
                throw new InvalidOperationException($"{newHero.CodeName ?? newHero.FirstName} " +
                    $"is too powerful to have a secondary power.");
            }
        }

        //Check to see if superhuman.IsLeader is true if it is then save the new information using CodeName
        public async Task AddSuperhumanWithLeaderAsync(Superhuman superhuman)
        {
            if (superhuman.IsTeamLeader != null && superhuman.IsTeamLeader == true)
            {
                var teamLeaderName = superhuman.CodeName ?? ($"{ superhuman.FirstName} {superhuman.LastName}");
                Teams currentTeam = await _teamsRepository.GetTeamByIdAsync((int)superhuman.TeamId);
                currentTeam.TeamLeader = teamLeaderName;
                await _teamsRepository.UpdateTeamAsync(currentTeam);
            }
        }
    }
}
