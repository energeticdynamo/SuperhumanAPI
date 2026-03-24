using SuperhumanAPI.Models;
namespace SuperhumanAPI.Repositories.Interfaces
{
    public interface IMutantRepository
    {
        public Task<PagedResult<Mutant>> GetAllMutantsAsync(int pageNumber, int pageSize);
        public Task<Mutant> GetMutantByIdAsync(int id);
        public Task AddMutantAsync(Mutant mutant);
        public Task UpdateMutantAsync(Mutant mutant);
        public Task<bool> DeleteMutantByMutantIdAsync(int id);
        public Task<bool> DoesMutantExistAsync(string firstName, string lastName);
    }
}
