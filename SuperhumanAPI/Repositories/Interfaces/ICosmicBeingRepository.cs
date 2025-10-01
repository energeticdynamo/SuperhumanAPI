using SuperhumanAPI.Models;
namespace SuperhumanAPI.Repositories.Interfaces
{
    public interface ICosmicBeingRepository
    {
        public Task<PagedResult<CosmicBeing>> GetAllCosmicBeingsAsync(int pageNumber, int pageSize);
        public Task<CosmicBeing> GetCosmicBeingByIdAsync(int id);
        public Task AddCosmicBeingAsync(CosmicBeing cosmicBeing);
        public Task UpdateCosmicBeingAsync(CosmicBeing cosmicBeing);
        public Task<bool> DeleteCosmicBeingByCosmicIdAsync(int id);
    }
}
