using SuperhumanAPI.Models;
namespace SuperhumanAPI.Repositories
{
    public interface IMutantRepository
    {
        public Task<IEnumerable<Mutant>> GetAllMutantsAsync();
        public Task<Mutant> GetMutantByIdAsync(int id);
        public Task AddMutantAsync(Mutant mutant);
        public Task UpdateMutantAsync(Mutant mutant);
        public Task<bool> DeleteMutantByMutantIdAsync(int id);
    }
}
