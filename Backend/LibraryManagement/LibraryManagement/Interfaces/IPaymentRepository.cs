using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface IPaymentRepository
    {
        Task<(IEnumerable<Payment>, int)> GetAll(int pageNumber, int pageSize);
        Task<Payment> GetByID(int paymentID);
        Task<int> Add(PaymentInsertModel payment);
        Task<int> UpdatePaymentStatus(int paymentID, string status, bool depositRefunded);
        Task<int> Delete(int paymentID);
    }
}