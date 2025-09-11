using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            return entity;
        }

        public async Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            return new PagedResult<T> { Items = items, TotalCount = totalCount };
        }

        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
