using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface IBookRepository
    {
        Task<(IEnumerable<Book>,int)> GetAll(string? title, int pageNumber, int pageSize);
        Task<Book> GetByID(int bookID);
        Task<int> Add(BookInsertModel book);
        Task<int> Update(BookUpdateModel book);
        Task<int> Delete(int bookID);
    }
}
