using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public interface ITeams
    {
        Task<IEnumerable<Teams>> GetAllActiveTeamsAsync();
    }
}
