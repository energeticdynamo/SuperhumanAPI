using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SuperhumanAPI.Data;
using SuperhumanAPI.Models;
using SuperhumanAPI.Repositories.Interfaces;

namespace SuperhumanAPI.Repositories.Implementations
{
    public class SuperhumanRepository : ISuperhumanRepository
    {
        //This is an example of dependency injection in the api.
        private readonly SuperhumanContext _context;
        public SuperhumanRepository(SuperhumanContext context)
        {
            _context = context;
        }
        public async Task AddSuperhumanAsync(Superhuman superhuman)
        {
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("AddSuperhuman", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FirstName", superhuman.FirstName);
                    command.Parameters.AddWithValue("@LastName", superhuman.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CodeName", superhuman.CodeName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PrimaryPower", superhuman.PrimaryPower);
                    command.Parameters.AddWithValue("@SecondaryPower", superhuman.SecondaryPower ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Ranking", superhuman.Ranking);
                    command.Parameters.AddWithValue("@TeamId", superhuman.TeamId ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> DeleteSuperhumanByIdAsync(int id)
        {
            var connectionString = _context.Database.GetConnectionString();

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("DeleteSuperhumanRecordById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@SuperhumanId", id);

                        await command.ExecuteNonQueryAsync();

                        return true;
                    }
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<PagedResult<Superhuman>> GetAllSuperhumansAsync(int pageNumber, int pageSize)
        {
            var superhumans = new List<Superhuman>();
            int totalCount = 0;
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetSuperhumanRecords", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var superhuman = new Superhuman
                            {
                                SuperhumanId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                CodeName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                PrimaryPower = reader.GetString(4),
                                SecondaryPower = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Ranking = reader.GetInt32(6),
                                TeamName = reader.IsDBNull(7) ? null : reader.GetString(7),
                            };

                            if (totalCount == 0)
                            {
                                totalCount = reader.GetInt32(reader.GetOrdinal("TotalCount"));
                            }
                            superhumans.Add(superhuman);
                        }
                    }
                }
            }

            return new PagedResult<Superhuman> { Items = superhumans, TotalCount = totalCount };
        }

        public async Task<Superhuman> GetSuperhumanByIdAsync(int id)
        {
            Superhuman? superhuman = null;
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetSuperhumanRecordById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@SuperhumanId", SqlDbType.Int) { Value = id });

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            superhuman = new Superhuman
                            {
                                SuperhumanId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                CodeName = reader.IsDBNull(3) ? null : reader.GetString(3),
                                PrimaryPower = reader.GetString(4),
                                SecondaryPower = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Ranking = reader.GetInt32(6),
                                TeamId = reader.GetInt32(7),
                            };
                        }
                    }
                }
            }

            if (superhuman == null)
            {
                throw new NullReferenceException($"Superhuman with ID {id} not found.");
            }

            return superhuman;
        }

        public async Task UpdateSuperhumanAsync(Superhuman superhuman)
        {
            var connectionString = _context.Database.GetConnectionString();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UpdateSuperhumanRecordById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SuperhumanId", superhuman.SuperhumanId);
                    command.Parameters.AddWithValue("@FirstName", superhuman.FirstName);
                    command.Parameters.AddWithValue("@LastName", superhuman.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@CodeName", superhuman.CodeName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PrimaryPower", superhuman.PrimaryPower);
                    command.Parameters.AddWithValue("@SecondaryPower", superhuman.SecondaryPower ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Ranking", superhuman.Ranking);
                    command.Parameters.AddWithValue("@TeamId", superhuman.TeamId ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
