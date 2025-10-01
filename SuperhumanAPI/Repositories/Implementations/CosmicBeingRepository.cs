using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Repositories.Implementations
{
    public class CosmicBeingRepository : ICosmicBeingRepository
    {
        private readonly CosmicBeingContext _context;
        public CosmicBeingRepository(CosmicBeingContext context)
        {
            _context = context;
        }
        public async Task AddCosmicBeingAsync(CosmicBeing cosmicBeing)
        {
            _context.CosmicBeings.Add(cosmicBeing);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception if necessary
                throw new InvalidOperationException("Error adding cosmic being to the database.", ex);
            }
        }

        public async Task<bool> DeleteCosmicBeingByCosmicIdAsync(int id)
        {
            var cosmicBeing = await _context.CosmicBeings.FindAsync(id);
            if (cosmicBeing == null)
                return false;
            _context.CosmicBeings.Remove(cosmicBeing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<CosmicBeing>> GetAllCosmicBeingsAsync(int pageNumber, int pageSize)
        {
            var cosmicBeings = await _context.CosmicBeings
                .FromSqlInterpolated($"EXEC GetAllCosmicBeings {pageNumber}, {pageSize}")
                .AsNoTracking()
                .ToListAsync();
            var totalCount = await _context.CosmicBeings.CountAsync();

            return new PagedResult<CosmicBeing> { Items = cosmicBeings, TotalCount = totalCount };
        }

        public async Task<CosmicBeing> GetCosmicBeingByIdAsync(int id)
        {
            var cosmicBeing = await _context.CosmicBeings
                .FromSqlInterpolated($"EXEC GetCosmicBeingsById {id}")
                .AsNoTracking()
                .ToListAsync();

            var result = cosmicBeing.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Cosmic Being not found");

            return result;
        }

        public async Task UpdateCosmicBeingAsync(CosmicBeing cosmicBeing)
        {
            _context.Entry(cosmicBeing).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Error updating cosmic being in the database.", ex);
            }
        }
    }
}
