using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public class SuperhumanRepository : EfRepository<Superhuman>, ISuperhumanRepository
    {
        public SuperhumanRepository(SuperhumanContext context) :base(context)
        {
        }
    }
}
