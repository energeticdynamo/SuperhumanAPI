using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
    }
}
