using SuperhumanAPI.Repositories.Implementations;
using SuperhumanAPI.Repositories.Interfaces;
using SuperhumanAPI.Services.Interfaces;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Writers;
using System.Security.Claims;

namespace SuperhumanAPI.Services.Implementations
{
    public class SuperhumanService : ISuperhumanService
    {
        private readonly ISuperhumanRepository _superhumanRepository;
        private readonly IMutantRepository _mutantRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IHttpContextAccessor _httpContextAccesses;

        public SuperhumanService(ISuperhumanRepository superhumanRepository, 
            IMutantRepository mutantRepository, 
            ITeamsRepository teamsRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _superhumanRepository = superhumanRepository;
            _mutantRepository = mutantRepository;
            _teamsRepository = teamsRepository;
            _httpContextAccesses = httpContextAccessor;
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
            if (newHero.TeamId is null)
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
            if (newHero.Ranking is 10 && newHero.SecondaryPower is not (null or ""))
            {
                throw new InvalidOperationException($"{newHero.CodeName ?? newHero.FirstName} " +
                    $"is too powerful to have a secondary power.");
            }
        }

        
        public async Task AddSuperhumanWithLeaderAsync(Superhuman superhuman)
        {
            await _superhumanRepository.AddSuperhumanAsync(superhuman);

            if (superhuman.IsTeamLeader is true)
            {
                if (superhuman.TeamId is not int teamId)
                {
                    throw new InvalidOperationException("A team leader must be assigned to a team.");
                }

                var currentTeam = await _teamsRepository.GetTeamByIdAsync(teamId);
                currentTeam.TeamLeader = superhuman.CodeName ?? $"{superhuman.FirstName} {superhuman.LastName}";
                await _teamsRepository.UpdateTeamAsync(currentTeam);
            }
        }

        public async Task AddSuperhumanWithAuditAsync(Superhuman superhuman)
        {
            var currentUserName = _httpContextAccesses.HttpContext?.User?.Identity?.Name ?? "System_Auto";

            superhuman.CreatedBy = currentUserName;
            superhuman.CreatedDate = DateTime.UtcNow;

            await AddSuperhumanWithLeaderAsync (superhuman);
        }
    }
}
