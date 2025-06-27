using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;

namespace SuperhumanAPI.Repositories
{
    public class TeamsRepository : ITeams
    {
        private readonly TeamContext _context;
        public TeamsRepository(TeamContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Teams>> GetAllActiveTeamsAsync()
        {
            var teams = new List<Teams>();
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetAllActiveTeams", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var team = new Teams
                            {
                                TeamId = reader.GetInt32(reader.GetOrdinal("TeamId")),
                                TeamName = reader.GetString(reader.GetOrdinal("TeamName")),
                                BaseOfOperations = reader.IsDBNull(reader.GetOrdinal("BaseOfOperations")) ? null : reader.GetString(reader.GetOrdinal("BaseOfOperations")),
                            };
                            teams.Add(team);
                        }
                    }
                }
            }
            return teams;
        }
    }
}
