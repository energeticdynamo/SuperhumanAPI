using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public interface ISuperhumanRepository
    {
        Task<IEnumerable<Superhuman>> GetAllSuperhumansAsync();
        Task<Superhuman> GetSuperhumanByIdAsync(int id);
        Task AddSuperhumanAsync(Superhuman superhuman);
        Task UpdateSuperhumanAsync(Superhuman superhuman);
        Task<bool> DeleteSuperhumanByIdAsync(int id);
    }
}
