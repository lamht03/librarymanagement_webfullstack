﻿using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LibraryManagement.Repositories
{
    public class SysFunctionInGroupRepository : ISysFunctionInGroupRepository
    {
        private readonly Connection _connectionString;

        public SysFunctionInGroupRepository(IConfiguration configuration)
        {
            _connectionString = new Connection();
        }

        public async Task<IEnumerable<SysFunctionInGroup>> GetAll()
        {
            var functionInGroupList = new List<SysFunctionInGroup>();

            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("FIG_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            functionInGroupList.Add(new SysFunctionInGroup
                            {
                                FunctionInGroupID = reader.GetInt32(reader.GetOrdinal("FunctionInGroupID")),
                                FunctionID = reader.GetInt32(reader.GetOrdinal("FunctionID")),
                                GroupID = reader.GetInt32(reader.GetOrdinal("GroupID")),
                                Permission = reader.GetInt32(reader.GetOrdinal("Permission"))
                            });
                        }
                    }
                }
            }

            return functionInGroupList;
        }

        public async Task<IEnumerable<SysFunctionInGroup>> GetByGroupID(int groupID)
        {
            var functionInGroupList = new List<SysFunctionInGroup>();

            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("FIG_GetByGroupID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GroupID", groupID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            functionInGroupList.Add(new SysFunctionInGroup
                            {
                                FunctionInGroupID = reader.GetInt32(reader.GetOrdinal("FunctionInGroupID")),
                                FunctionID = reader.GetInt32(reader.GetOrdinal("FunctionID")),
                                GroupID = reader.GetInt32(reader.GetOrdinal("GroupID")),
                                Permission = reader.GetInt32(reader.GetOrdinal("Permission"))
                            });
                        }
                    }
                }
            }

            return functionInGroupList;
        }

        public async Task<IEnumerable<SysFunctionInGroup>> GetByFunctionID(int functionID)
        {
            var functionInGroupList = new List<SysFunctionInGroup>();

            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("FIG_GetByFunctionID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionID", functionID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            functionInGroupList.Add(new SysFunctionInGroup
                            {
                                FunctionInGroupID = reader.GetInt32(reader.GetOrdinal("FunctionInGroupID")),
                                FunctionID = reader.GetInt32(reader.GetOrdinal("FunctionID")),
                                GroupID = reader.GetInt32(reader.GetOrdinal("GroupID")),
                                Permission = reader.GetInt32(reader.GetOrdinal("Permission"))
                            });
                        }
                    }
                }
            }

            return functionInGroupList;
        }

        public async Task<SysFunctionInGroup> GetByID(int functionInGroupID)
        {
            SysFunctionInGroup functionInGroup = null;

            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("FIG_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionInGroupID", functionInGroupID);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            functionInGroup = new SysFunctionInGroup
                            {
                                FunctionInGroupID = reader.GetInt32(reader.GetOrdinal("FunctionInGroupID")),
                                FunctionID = reader.GetInt32(reader.GetOrdinal("FunctionID")),
                                GroupID = reader.GetInt32(reader.GetOrdinal("GroupID")),
                                Permission = reader.GetInt32(reader.GetOrdinal("Permission"))
                            };
                        }
                    }
                }
            }

            return functionInGroup;
        }

        public async Task<int> Create(SysFunctionInGroupInsertModel functionInGroup)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var command = new SqlCommand("FIG_Create", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionID", functionInGroup.FunctionID);
                    command.Parameters.AddWithValue("@GroupID", functionInGroup.GroupID);
                    command.Parameters.AddWithValue("@Permission", functionInGroup.Permission);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(SysFunctionInGroupUpdateModel functionInGroup)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var command = new SqlCommand("FIG_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionInGroupID", functionInGroup.FunctionInGroupID);
                    command.Parameters.AddWithValue("@FunctionID", functionInGroup.FunctionID);
                    command.Parameters.AddWithValue("@GroupID", functionInGroup.GroupID);
                    command.Parameters.AddWithValue("@Permission", functionInGroup.Permission);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int functionInGroupID)
        {
            using (var connection = new SqlConnection(_connectionString.GetConnectionString()))
            {
                using (var command = new SqlCommand("FIG_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FunctionInGroupID", functionInGroupID);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}