using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace LibraryManagement.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly Connection _connection;

        public BookRepository(IConfiguration configuration)
        {
            _connection = new Connection(); ;
        }

        public async Task<(IEnumerable<Book>, int)> GetAll(string? title, int pageNumber, int pageSize)
        {
            var books = new List<Book>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            using (var command = new SqlCommand("Books_GetAll", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Title", title ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PageNumber", pageNumber);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        books.Add(new Book
                        {
                            BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            Genre = reader.GetString(reader.GetOrdinal("Genre")),
                            PublishedDate = reader.GetDateTime(reader.GetOrdinal("PublishedDate")),
                            TotalQuantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                            Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString(reader.GetOrdinal("Description")) : null,
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                            BorrowedQuantity = reader.GetInt32(reader.GetOrdinal("BorrowedQuantity")),
                            AvailableQuantity = reader.GetInt32(reader.GetOrdinal("AvailableQuantity"))
                        });
                    }

                    // Chuyển sang kết quả thứ hai để lấy tổng số bản ghi
                    if (await reader.NextResultAsync() && await reader.ReadAsync())
                    {
                        totalRecords = reader.GetInt32(reader.GetOrdinal("TotalRecords"));
                    }
                }
            }

            return (books, totalRecords);
        }

        public async Task<Book> GetByID(int bookID)
        {
            Book book = null;
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            {
                using (var command = new SqlCommand("Books_GetByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookID", bookID);

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            book = new Book
                            {
                                BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Author = reader.GetString(reader.GetOrdinal("Author")),
                                Genre = reader.GetString(reader.GetOrdinal("Genre")),
                                PublishedDate = reader.GetDateTime(reader.GetOrdinal("PublishedDate")),
                                TotalQuantity = reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                                Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString(reader.GetOrdinal("Description")) : null,
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                BorrowedQuantity = reader.GetInt32(reader.GetOrdinal("BorrowedQuantity")),
                                AvailableQuantity = reader.GetInt32(reader.GetOrdinal("AvailableQuantity"))
                            };
                        }
                    }
                }
            }
            return book;
        }

        public async Task<int> Add(BookInsertModel book)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            {
                using (var command = new SqlCommand("Books_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Genre", book.Genre);
                    command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate);
                    command.Parameters.AddWithValue("@TotalQuantity", book.TotalQuantity);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Update(BookUpdateModel book)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            {
                using (var command = new SqlCommand("Books_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookID", book.BookID);
                    command.Parameters.AddWithValue("@Title", book.Title);
                    command.Parameters.AddWithValue("@Author", book.Author);
                    command.Parameters.AddWithValue("@Genre", book.Genre);
                    command.Parameters.AddWithValue("@PublishedDate", book.PublishedDate);
                    command.Parameters.AddWithValue("@TotalQuantity", book.TotalQuantity);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> Delete(int bookID)
        {
            using (var connection = new SqlConnection(_connection.GetConnectionString()))
            {
                using (var command = new SqlCommand("Books_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@BookID", bookID);

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
