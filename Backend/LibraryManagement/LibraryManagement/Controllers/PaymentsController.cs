using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISysUserRepository _sysuserRepository;

        public PaymentsController(IPaymentRepository paymentRepository, ITransactionRepository transactionRepository, ISysUserRepository sysUserRepository)
        {
            _paymentRepository = paymentRepository;
            _transactionRepository = transactionRepository;
            _sysuserRepository = sysUserRepository;
        }

        [HttpGet("List")]
        [CustomAuthorize(1,"ManagePayments")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 20)
        {
            if (pageNumber <= 0)
                return BadRequest(new { message = "Invalid page number. Page number must be greater than 0." });

            if (pageSize <= 0 || pageSize > 50)
                return BadRequest(new { message = "Invalid page size. Page size must be between 1 and 50." });

            var (payments, totalRecords) = await _paymentRepository.GetAll(pageNumber, pageSize);
            if (!payments.Any())
                return Ok(new { Status = 0, Message = "No data available", Data = payments });

            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            return Ok(new
            {
                Status = 1,
                Message = "Payments retrieved successfully",
                Data = payments,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("Get Payment By ID")]
        [CustomAuthorize(1, "ManagePayments")]
        public async Task<IActionResult> GetByID(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid payment ID." });

            var payment = await _paymentRepository.GetByID(id);
            if (payment == null)
                return NotFound(new { message = "Payment not found." });

            return Ok(new { Status = 1, Message = "Payment retrieved successfully", Data = payment });
        }

        [HttpPost("Add Payment")]
        [CustomAuthorize(2, "ManagePayments")]
        public async Task<IActionResult> Add(PaymentInsertModel payment)
        {
            var existingTransaction = _transactionRepository.GetByID(payment.TransactionID);
            if (payment.TransactionID <= 0 || existingTransaction == null)
                return BadRequest(new { message = "Invalid Transaction ID." });

            var existingUser = await _sysuserRepository.GetByID(payment.UserID);
            if (payment.UserID <= 0 || existingUser == null)
                return BadRequest(new { message = "Invalid User ID." });

            if (payment.Amount <= 0)
                return BadRequest(new { message = "Amount must be greater than zero." });

            int rowsAffected = await _paymentRepository.Add(payment);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while adding the payment." });

            return Ok(new { message = "Payment added successfully." });
        }

        [HttpPut("Update Status Of Payment")]
        [CustomAuthorize(4, "ManagePayments")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentID, string status, bool depositRefunded)
        {
            if (paymentID <= 0)
                return BadRequest(new { message = "Invalid payment ID." });

            var existingPayment = await _paymentRepository.GetByID(paymentID);
            if (existingPayment == null)
                return NotFound(new { message = "Payment not found." });

            if (string.IsNullOrWhiteSpace(status))
                return BadRequest(new { message = "Payment status is required." });

            int rowsAffected = await _paymentRepository.UpdatePaymentStatus(paymentID, status, depositRefunded);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while updating payment status." });

            return Ok(new { message = "Payment status updated successfully." });
        }

        [HttpDelete("Delete Payment")]
        [CustomAuthorize(8, "ManagePayments")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid payment ID." });

            var existingPayment = await _paymentRepository.GetByID(id);
            if (existingPayment == null)
                return NotFound(new { message = "Payment not found." });

            int rowsAffected = await _paymentRepository.Delete(id);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while deleting the payment." });

            return Ok(new { message = "Payment deleted successfully." });
        }
    }
}
