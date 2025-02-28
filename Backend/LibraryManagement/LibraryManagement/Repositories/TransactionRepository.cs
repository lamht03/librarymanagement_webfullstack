// Transaction Repository
using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace LibraryManagement.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly Connection _connection;

        public TransactionRepository(IConfiguration configuration)
        {
            _connection = new Connection();
        }

        public async Task<(IEnumerable<Transaction>, int)> GetAll(int pageNumber, int pageSize)
        {
            var transactions = new List<Transaction>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Transactions_GetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        transactions.Add(new Transaction
                        {
                            TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                            DepositAmount = reader.GetDecimal(reader.GetOrdinal("DepositAmount")),
                            Status = reader.GetString(reader.GetOrdinal("Status"))
                        });
                    }

                    if (await reader.NextResultAsync() && await reader.ReadAsync())
                    {
                        totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                    }
                }
            }

            return (transactions, totalRecords);
        }

        public async Task<Transaction> GetByID(int transactionID)
        {
            Transaction transaction = null;
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Transactions_GetByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionID", transactionID);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        transaction = new Transaction
                        {
                            TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            BorrowDate = reader.GetDateTime(reader.GetOrdinal("BorrowDate")),
                            DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                            ReturnDate = reader.IsDBNull(reader.GetOrdinal("ReturnDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ReturnDate")),
                            DepositAmount = reader.GetDecimal(reader.GetOrdinal("DepositAmount")),
                            Status = reader.GetString(reader.GetOrdinal("Status"))
                        };
                    }
                }
            }
            return transaction;
        }

        public async Task<int> Add(TransactionInsertModel transaction)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Transactions_Insert", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@BookID", transaction.BookID);
                command.Parameters.AddWithValue("@UserID", transaction.UserID);
                command.Parameters.AddWithValue("@BorrowDate", transaction.BorrowDate);
                command.Parameters.AddWithValue("@DueDate", transaction.DueDate);
                command.Parameters.AddWithValue("@DepositAmount", transaction.DepositAmount);
                command.Parameters.AddWithValue("@Status", transaction.Status);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> Update(TransactionUpdateModel transaction)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Transactions_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionID", transaction.TransactionID);
                command.Parameters.AddWithValue("@ReturnDate", transaction.ReturnDate);
                command.Parameters.AddWithValue("@Status", transaction.Status);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> Delete(int transactionID)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Transactions_Delete", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionID", transactionID);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}