using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ISysUserRepository _sysUserRepository;

        public TransactionsController(ITransactionRepository transactionRepository, IBookRepository bookRepository, ISysUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _bookRepository = bookRepository;
            _sysUserRepository = userRepository;
        }

        [HttpGet("List")]
        [CustomAuthorize(1,"ManageTransactions")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber <= 0)
                return BadRequest(new { message = "Invalid page number. Page number must be greater than 0." });

            if (pageSize <= 0 || pageSize > 50)
                return BadRequest(new { message = "Invalid page size. Page size must be between 1 and 50." });

            var (transactions, totalRecords) = await _transactionRepository.GetAll(pageNumber, pageSize);
            if (!transactions.Any())
                return Ok(new { Status = 0, Message = "No data available", Data = transactions });

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            return Ok(new
            {
                Status = 1,
                Message = "Transactions retrieved successfully",
                Data = transactions,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("Get Transaction By ID")]
        [CustomAuthorize(1, "ManageTransactions")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid transaction ID." });

            var transaction = await _transactionRepository.GetByID(id);
            if (transaction == null)
                return NotFound(new { message = "Transaction not found." });

            return Ok(new { Status = 1, Message = "Transaction retrieved successfully", Data = transaction });
        }

        [HttpPost("Add Transaction")]
        [CustomAuthorize(2, "ManageTransactions")]
        public async Task<IActionResult> Add(TransactionInsertModel transaction)
        {
            var existingBook = await _bookRepository.GetByID(transaction.BookID);
            if (transaction.BookID <= 0 || existingBook == null)
                return BadRequest(new { message = "Invalid Book ID." });

            var existingUser = await _sysUserRepository.GetByID(transaction.UserID);
            if (transaction.UserID <= 0 ||  existingUser == null)
                return BadRequest(new { message = "Invalid User ID." });

            if (transaction.DueDate <= transaction.BorrowDate)
                return BadRequest(new { message = "Due date must be after the borrow date." });

            if (transaction.DepositAmount <= 0)
                return BadRequest(new { message = "Deposit amount must be greater than zero." });

            int rowsAffected = await _transactionRepository.Add(transaction);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while adding the transaction." });

            return Ok(new { message = "Transaction added successfully." });
        }


        [HttpPut("Update Transaction")]
        [CustomAuthorize(4, "ManageTransactions")]
        public async Task<IActionResult> Update(TransactionUpdateModel transaction)
        {
            if (transaction.TransactionID <= 0)
                return BadRequest(new { message = "Invalid transaction ID." });

            var existingTransaction = await _transactionRepository.GetByID(transaction.TransactionID);
            if (existingTransaction == null)
                return NotFound(new { message = "Transaction not found." });

            if (transaction.DueDate <= transaction.BorrowDate)
                return BadRequest(new { message = "Due date must be after the borrow date." });

            int rowsAffected = await _transactionRepository.Update(transaction);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while updating the transaction." });

            return Ok(new { message = "Transaction updated successfully." });
        }


        [HttpDelete("Delete Transaction")]
        [CustomAuthorize(8, "ManageTransactions")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid transaction ID." });

            var existingTransaction = await _transactionRepository.GetByID(id);
            if (existingTransaction == null)
                return NotFound(new { message = "Transaction not found." });

            int rowsAffected = await _transactionRepository.Delete(id);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while deleting the transaction." });

            return Ok(new { message = "Transaction deleted successfully." });
        }
    }
}
