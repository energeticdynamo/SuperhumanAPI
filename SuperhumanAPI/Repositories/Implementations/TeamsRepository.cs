using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Repositories.Implementations
{
    public class TeamsRepository : ITeamsRepository
    {
        private readonly TeamContext _context;
        public TeamsRepository(TeamContext context)
        {
            _context = context;
        }

        public async Task AddTeamAsync(Teams team)
        {
            _context.Teams.Add(team);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception if necessary
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
            return await _context.Teams
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Teams> GetTeamByIdAsync(int id)
        {
            var team = await _context.Teams
                .FromSqlInterpolated($"EXEC GetTeamRecordById {id}")
                .AsNoTracking()
                .ToListAsync();
            var result = team.FirstOrDefault();

            if (result == null)
            {
                throw new KeyNotFoundException($"Team with ID {id} not found.");
            }

            return result;
        }

        public async Task UpdateTeamAsync(Teams team)
        {
            _context.Entry(team).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the exception if necessary
                throw new InvalidOperationException("Error updating team in the database.", ex);
            }
        }
    }
}
