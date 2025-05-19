using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using System.Collections.Generic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SuperhumanAPI.Repositories
{
    public class MutantRepository : IMutantRepository
    {
        private readonly SuperhumanContext _context;
        public MutantRepository(SuperhumanContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Mutant>> GetAllMutantsAsync()
        {
            var mutants = new List<Mutant>();
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetMutantRecords", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var mutant = new Mutant
                            {
                                MutantId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                MutantName = reader.GetString(3),
                                PrimaryPower = reader.GetString(4),
                                SecondaryPower = reader.GetString(5),
                                TeamAffiliation = reader.GetString(6),
                                Classification = reader.GetString(7)

                            };
                            mutants.Add(mutant);
                        }
                    }
                }
            }

            return mutants;
        }
        public async Task<Mutant> GetMutantByIdAsync(int id)
        {
            var mutant = new Mutant();
            var connectionString = _context.Database.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetMutantRecordById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@MutantId", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            
                            mutant.MutantId = reader.GetInt32(0);
                            mutant.FirstName = reader.GetString(1);
                            mutant.LastName = reader.GetString(2);
                            mutant.MutantName = reader.GetString(3);
                            mutant.PrimaryPower = reader.GetString(4);
                            mutant.SecondaryPower = reader.GetString(5);
                            mutant.TeamAffiliation = reader.GetString(6);
                            mutant.Classification = reader.GetString(7);

                        }
                    }
                }
            }
            return mutant;
        }
        public async Task AddMutantAsync(Mutant mutant)
        {
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("AddingMutant", connection))
                {
                    //command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@FirstName", mutant.FirstName);
                    //command.Parameters.AddWithValue("@LastName", mutant.LastName ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@CodeName", mutant.MutantName ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@PrimaryPower", mutant.PrimaryPower);
                    //command.Parameters.AddWithValue("@SecondaryPower", mutant.SecondaryPower ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@TeamAffiliation", mutant.TeamAffiliation ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@Ranking", mutant.Ranking);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task UpdateMutantAsync(Mutant mutant)
        {
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UpdatingMutant", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@SuperhumanId", mutant.SuperhumanId);
                    //command.Parameters.AddWithValue("@MutantId", mutant.MutantId);
                    //command.Parameters.AddWithValue("@FirstName", mutant.FirstName);
                    //command.Parameters.AddWithValue("@LastName", mutant.LastName ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@CodeName", mutant.MutantName ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@PrimaryPower", mutant.PrimaryPower);
                    //command.Parameters.AddWithValue("@SecondaryPower", mutant.SecondaryPower ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@TeamAffiliation", mutant.TeamAffiliation ?? (object)DBNull.Value);
                    //command.Parameters.AddWithValue("@Ranking", mutant.Ranking);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<bool> DeleteMutantByMutantIdAsync(int id)
        {
            var connectionString = _context.Database.GetConnectionString();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("DeleteMutantRecordById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@MutantId", id);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                // Log the exception if necessary
                return false;
            }
        }
    }
}
