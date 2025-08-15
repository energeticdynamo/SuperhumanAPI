using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperhumanAPI.Repositories
{
    public class MutantRepository : IMutantRepository
    {
        private readonly MutantContext _context;
        public MutantRepository(MutantContext context)
        {
            _context = context;
        }

        public async Task AddMutantAsync(Mutant mutant)
        {
            _context.Mutants.Add(mutant);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log the exception if necessary
                throw new InvalidOperationException("Error adding mutant to the database.", ex);
            }
        }

        public async Task<bool> DeleteMutantByMutantIdAsync(int id)
        {
            var mutant = await _context.Mutants.FindAsync(id);
            if (mutant == null)
                return false;
            _context.Mutants.Remove(mutant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Mutant>> GetAllMutantsAsync(int pageNumber, int pageSize)
        {
            var mutants = await _context.Mutants
                .FromSqlInterpolated($"EXEC GetMutantRecords {pageNumber}, {pageSize}")
                .AsNoTracking()
                .ToListAsync();
            
            var totalCount = await _context.Mutants.CountAsync();

            return new PagedResult<Mutant> { Items = mutants, TotalCount = totalCount };
        }

        public async Task<Mutant> GetMutantByIdAsync(int id)
        {
            var mutant = await _context.Mutants
                .FromSqlInterpolated($"EXEC GetMutantRecordById {id}")
                .AsNoTracking()
                .ToListAsync();

            var result = mutant.FirstOrDefault();

            if (result == null)
                throw new InvalidOperationException("Mutant not found.");

            return result;
        }

        public async Task UpdateMutantAsync(Mutant mutant)
        {
           _context.Entry(mutant).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log the exception if necessary
                throw new InvalidOperationException("Error updating mutant in the database.", ex);
            }
        }
        
    }
}
