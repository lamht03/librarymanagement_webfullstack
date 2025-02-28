using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace LibraryManagement.Repositories
{     
    public class PaymentRepository : IPaymentRepository
    {
        private readonly Connection _connection;

        public PaymentRepository(IConfiguration configuration)
        {
            _connection = new Connection();
        }

        public async Task<(IEnumerable<Payment>, int)> GetAll(int pageNumber, int pageSize)
        {
            var payments = new List<Payment>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Payments_GetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        payments.Add(new Payment
                        {
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                            TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                            PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                            PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                            DepositRefunded = reader.GetBoolean(reader.GetOrdinal("DepositRefunded"))
                        });
                    }

                    if (await reader.NextResultAsync() && await reader.ReadAsync())
                    {
                        totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                    }
                }
            }

            return (payments, totalRecords);
        }

        public async Task<Payment> GetByID(int paymentID)
        {
            Payment payment = null;
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Payments_GetByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PaymentID", paymentID);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        payment = new Payment
                        {
                            PaymentID = reader.GetInt32(reader.GetOrdinal("PaymentID")),
                            TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID")),
                            UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                            Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                            PaymentDate = reader.GetDateTime(reader.GetOrdinal("PaymentDate")),
                            PaymentMethod = reader.GetString(reader.GetOrdinal("PaymentMethod")),
                            PaymentStatus = reader.GetString(reader.GetOrdinal("PaymentStatus")),
                            DepositRefunded = reader.GetBoolean(reader.GetOrdinal("DepositRefunded"))
                        };
                    }
                }
            }
            return payment;
        }

        public async Task<int> Add(PaymentInsertModel payment)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Payments_Insert", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TransactionID", payment.TransactionID);
                command.Parameters.AddWithValue("@UserID", payment.UserID);
                command.Parameters.AddWithValue("@Amount", payment.Amount);
                command.Parameters.AddWithValue("@PaymentMethod", payment.PaymentMethod);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdatePaymentStatus(int paymentID, string status, bool depositRefunded)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Payments_UpdateStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PaymentID", paymentID);
                command.Parameters.AddWithValue("@PaymentStatus", status);
                command.Parameters.AddWithValue("@DepositRefunded", depositRefunded);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> Delete(int paymentID)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Payments_Delete", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PaymentID", paymentID);

                await connection.OpenAsync();
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
    