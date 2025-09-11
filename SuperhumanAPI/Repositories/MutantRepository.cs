using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperhumanAPI.Repositories
{
    public class MutantRepository :EfRepository<Mutant>, IMutantRepository
    {
        public MutantRepository(MutantContext context) : base(context)
        {
        }
    }
}
