using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Repositories.Implementations
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly TeamContext _context;
        private readonly IDistributedCache _cache;
        private static readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

        public TeamsRepository(TeamContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<Teams> GetTeamByIdAsync(int id)
        {
            var cacheKey = $"team_{id}";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached is not null)
                return JsonSerializer.Deserialize<Teams>(cached)!;

            var team = await _context.Teams
                .FromSqlInterpolated($"EXEC GetTeamRecordById {id}")
                .AsNoTracking()
                .ToListAsync();

            var result = team.FirstOrDefault();

            if (result is not null)
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheExpiry });
            }

            return result!;
        }

        public async Task AddTeamAsync(Teams team)
        {
            _context.Teams.Add(team);
            try
            {
                await _context.SaveChangesAsync();
                // Invalidate list caches so next read pulls fresh data
                await _cache.RemoveAsync("teams_all");
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Error adding team to the database.", ex);
            }
        }

        public async Task<bool> DeleteTeamByIdAsync(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return false;
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            // Invalidate caches
            await _cache.RemoveAsync($"team_{id}");
            await _cache.RemoveAsync("teams_all");
            return true;
        }

        public async Task<PagedResult<Teams>> GetAllActiveTeamsAsync(int pageNumber, int pageSize)
        {
            var teams = await _context.Teams
                 .FromSqlInterpolated($"EXEC GetTeamRecords {pageNumber}, {pageSize}")
                 .AsNoTracking()
                 .ToListAsync();

            var totalCount = await _context.Teams.CountAsync();

            return new PagedResult<Teams> { Items = teams, TotalCount = totalCount };
        }

        public async Task<IEnumerable<Teams>> GetAllTeamsAsync()
        {
            var cacheKey = "teams_all";
            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached is not null)
                return JsonSerializer.Deserialize<List<Teams>>(cached)!;

            var teams = await _context.Teams
                .AsNoTracking()
                .ToListAsync();
            
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(teams),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = CacheExpiry });

            return teams;

        }


        public async Task UpdateTeamAsync(Teams team)
        {
            _context.Entry(team).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();

                // Invalidate caches
                await _cache.RemoveAsync($"team_{team.TeamId}");
                await _cache.RemoveAsync("teams_all");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the exception if necessary
                throw new InvalidOperationException("Error updating team in the database.", ex);
            }
        }
    }
}
