using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface ITransactionRepository
    {
        Task<(IEnumerable<Transaction>, int)> GetAll(int pageNumber, int pageSize);
        Task<Transaction> GetByID(int transactionID);
        Task<int> Add(TransactionInsertModel transaction);
        Task<int> Update(TransactionUpdateModel transaction);
        Task<int> Delete(int transactionID);
    }
}