using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public interface ISuperhumanRepository
    {
        Task<PagedResult<Superhuman>> GetAllSuperhumansAsync(int pageNumber, int pageSize);
        Task<Superhuman> GetSuperhumanByIdAsync(int id);
        Task AddSuperhumanAsync(Superhuman superhuman);
        Task UpdateSuperhumanAsync(Superhuman superhuman);
        Task<bool> DeleteSuperhumanByIdAsync(int id);
    }
}
