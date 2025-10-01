using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public interface ITeamsRepository
    {
        public Task<PagedResult<Teams>> GetAllActiveTeamsAsync(int pageNumber, int pageSize);
        public Task<Teams> GetTeamByIdAsync(int id);
        public Task AddTeamAsync(Teams team);
        public Task UpdateTeamAsync(Teams team);
        public Task<bool> DeleteTeamByIdAsync(int id);
    }
}
