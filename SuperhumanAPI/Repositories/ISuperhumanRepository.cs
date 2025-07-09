using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public interface ISuperhumanRepository
    {
        Task<PagedResult<SuperhumanDTO>> GetAllSuperhumansAsync(int pageNumber, int pageSize);
        Task<SuperhumanDTO> GetSuperhumanByIdAsync(int id);
        Task AddSuperhumanAsync(SuperhumanDTO superhuman);
        Task UpdateSuperhumanAsync(SuperhumanDTO superhuman);
        Task<bool> DeleteSuperhumanByIdAsync(int id);
    }
}
