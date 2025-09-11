using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}
