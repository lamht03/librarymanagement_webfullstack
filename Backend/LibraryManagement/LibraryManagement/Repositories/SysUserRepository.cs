using LibraryManagement.Interfaces;
using System.Data.SqlClient;
using System.Data;
using LibraryManagement.Utilities;
using LibraryManagement.Models;
using LibraryManagement.Interfaces;

namespace LibraryManagement.Repositories
{
    public class SysUserRepository : ISysUserRepository
    {
        private readonly Connection _connectionString;

        public SysUserRepository(IConfiguration configuration)
        {
            _connectionString = new Connection();
        }

        public async Task<(IEnumerable<SysUser>, int)> GetAll(string? userName, int pageNumber, int pageSize)
        {
            var userList = new List<SysUser>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("UMS_GetListPaging", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Đọc dữ liệu người dùng
                        while (await reader.ReadAsync())
                        {
                            userList.Add(new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                FullName = !reader.IsDBNull(reader.GetOrdinal("FullName")) ? reader.GetString(reader.GetOrdinal("FullName")) : null,
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = !reader.IsDBNull(reader.GetOrdinal("Note")) ? reader.GetString(reader.GetOrdinal("Note")) : null,
                            });
                        }

                        // Đọc tổng số bản ghi từ truy vấn riêng biệt
                        await reader.NextResultAsync(); // Di chuyển đến kết quả thứ hai
                        if (await reader.ReadAsync())
                        {
                            totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                        }
                    }
                }
            }

            return (userList, totalRecords);
        }


        public async Task<SysUser> GetByID(int userId)
        {
            SysUser user = null;
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var cmd = new SqlCommand("UMS_GetByID", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                FullName = !reader.IsDBNull(reader.GetOrdinal("FullName")) ? reader.GetString(reader.GetOrdinal("FullName")) : null,
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = !reader.IsDBNull(reader.GetOrdinal("Note")) ? reader.GetString(reader.GetOrdinal("Note")) : null
                            };
                        }
                    }
                }
            }

            return user;
        }


        public async Task<SysUser> GetByEmail(string email)
        {
            SysUser user = null;
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var cmd = new SqlCommand("UMS_GetByEmail", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    await connection.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                FullName = !reader.IsDBNull(reader.GetOrdinal("FullName")) ? reader.GetString(reader.GetOrdinal("FullName")) : null,
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = !reader.IsDBNull(reader.GetOrdinal("Note")) ? reader.GetString(reader.GetOrdinal("Note")) : null
                            };
                        }
                    }
                }
            }

            return user;
        }

        public async Task<int> Create(SysUserInsertModel user)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var cmd = new SqlCommand("UMS_Create", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Status", user.Status);
                    cmd.Parameters.AddWithValue("@Note", user.Note);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Register(RegisterModel user)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var cmd = new SqlCommand("UMS_Create", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Status", user.Status);
                    cmd.Parameters.AddWithValue("@Note", user.Note);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();

                }
            }
        }

        public async Task<int> Update(SysUserUpdateModel user)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var cmd = new SqlCommand("UMS_Update", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Status", user.Status);
                    cmd.Parameters.AddWithValue("@Note", user.Note);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        public async Task<int> Delete(int userId)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var cmd = new SqlCommand("UMS_Delete", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    await connection.OpenAsync();
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<SysUser> GetByRefreshToken(string refreshToken)
        {
            SysUser user = null;
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var command = new SqlCommand("UMS_GetByRefreshToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                            };
                        }
                    }
                }
            }
            return user;
        }

        public async Task<SysUser> VerifyLogin(string userName, string password)
        {
            SysUser user = null;

            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var command = new SqlCommand("VerifyLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new SysUser
                            {
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                FullName = !reader.IsDBNull(reader.GetOrdinal("FullName")) ? reader.GetString(reader.GetOrdinal("FullName")) : null,
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status")),
                                Note = !reader.IsDBNull(reader.GetOrdinal("Note")) ? reader.GetString(reader.GetOrdinal("Note")) : null
                            };
                        }
                    }
                }
            }

            return user;
        }

        // 1. Lưu session mới vào cơ sở dữ liệu
        public async Task CreateSession(int userId, string refreshToken, DateTime expiryDate)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("CreateSession", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);
                    command.Parameters.AddWithValue("@ExpiryDate", expiryDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 2. Lấy session theo refresh token
        public async Task<Session> GetSessionByRefreshToken(string refreshToken)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("GetSessionByRefreshToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RefreshToken", refreshToken);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Session
                            {
                                SessionID = reader.GetInt32(reader.GetOrdinal("SessionID")),
                                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                RefreshToken = reader.GetString(reader.GetOrdinal("RefreshToken")),
                                ExpiryDate = reader.GetDateTime(reader.GetOrdinal("ExpiryDate")),
                                IsRevoked = reader.GetBoolean(reader.GetOrdinal("IsRevoked")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // 3. Cập nhật refresh token cho session
        public async Task UpdateSessionRefreshToken(int sessionId, string newRefreshToken, DateTime newExpiryDate)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("UpdateSessionRefreshToken", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SessionID", sessionId);
                    command.Parameters.AddWithValue("@NewRefreshToken", newRefreshToken);
                    command.Parameters.AddWithValue("@NewExpiryDate", newExpiryDate);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 4. Thu hồi session
        public async Task RevokeSession(int sessionId)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("RevokeSession", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SessionID", sessionId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 5. Thu hồi tất cả session của người dùng
        public async Task RevokeAllSessions(int userId)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("RevokeAllSessions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 6. Xóa tất cả session của người dùng
        public async Task DeleteAllSessions(int userId)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("DeleteAllSessions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }


    }



}
