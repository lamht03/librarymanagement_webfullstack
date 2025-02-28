using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet("BooksList")]
        [CustomAuthorize(1,"ManageBooks")]
        public async Task<IActionResult> GetAll(string? title, int pageNumber = 1, int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                title = title.Trim();
            }

            if (pageNumber <= 0)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid page number. Page number must be greater than 0."
                });
            }

            if (pageSize <= 0 || pageSize > 50)
            {
                return BadRequest(new Response
                {
                    Status = 0,
                    Message = "Invalid page size. Page size must be between 1 and 50."
                });
            }

            var (books, totalRecords) = await _bookRepository.GetAll(title, pageNumber, pageSize);
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (!books.Any())
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "No data available",
                    Data = books
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = books,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            });
        }

        [HttpGet("Search books by ID")]
        [CustomAuthorize(1, "ManageBooks")]
        public async Task<IActionResult> GetByID(int bookId)
        {
            var book = await _bookRepository.GetByID(bookId);
            if (book == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "Id not found",
                    Data = book
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Get information successfully",
                Data = book
            });
        }

        [HttpPost("Add Book")]
        [CustomAuthorize(2, "ManageBooks")]
        public async Task<IActionResult> Add(BookInsertModel book)
        {
            if (string.IsNullOrWhiteSpace(book.Title) || book.Title.Length > 255)
                return BadRequest(new { message = "Invalid book title. Title is required and must not exceed 255 characters." });

            var existingTitle = await _bookRepository.GetAll(book.Title, 1, 20);
            if (existingTitle.Item1.Any())
            {
                return BadRequest(new Response { Status = 0, Message = "Title already exists. Please choose a different Title." });
            }

            if (string.IsNullOrWhiteSpace(book.Author) || book.Author.Length > 255)
                return BadRequest(new { message = "Invalid author name. Author is required and must not exceed 255 characters." });

            if (book.TotalQuantity < 0)
                return BadRequest(new { message = "Total quantity must be greater than or equal to 0." });

            if (book.Description?.Length > 500)
                return BadRequest(new { message = "Description must not exceed 500 characters." });

            if (book.Genre?.Length > 100)
                return BadRequest(new { message = "Genre must not exceed 100 characters." });

            int rowsAffected = await _bookRepository.Add(book);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while creating the book." });

            return Ok(new { message = "Book added successfully." });
        }

        [HttpPost("Update Book")]
        [CustomAuthorize(4, "ManageBooks")]
        public async Task<IActionResult> Update(BookUpdateModel book)
        {
            if (book.BookID <= 0)
                return BadRequest(new { message = "Invalid book ID." });

            var existingBook = await _bookRepository.GetByID(book.BookID);
            if (existingBook == null)
                return NotFound(new { message = "Book not found." });

            if (existingBook.Title != book.Title)
            {
                var existingBookName = await _bookRepository.GetAll(book.Title, 1, 20);
                if (existingBookName.Item1.Any())
                {
                    return BadRequest(new Response { Status = 0, Message = "Username already exists. Please choose a different username." });
                }
            }


            if (string.IsNullOrWhiteSpace(book.Title) || book.Title.Length > 255)
                return BadRequest(new { message = "Invalid book title. Title is required and must not exceed 255 characters." });

            if (string.IsNullOrWhiteSpace(book.Author) || book.Author.Length > 255)
                return BadRequest(new { message = "Invalid author name. Author is required and must not exceed 255 characters." });

            if (book.TotalQuantity < 0)
                return BadRequest(new { message = "Total quantity must be greater than or equal to 0." });

            if (book.Description?.Length > 500)
                return BadRequest(new { message = "Description must not exceed 500 characters." });

            if (book.Genre?.Length > 100)
                return BadRequest(new { message = "Genre must not exceed 100 characters." });

            int rowsAffected = await _bookRepository.Update(book);
            if (rowsAffected == 0)
                return StatusCode(500, new { message = "An error occurred while updating the book." });

            return Ok(new { message = "Book updated successfully." });
        }

        [HttpPost("Delete Book")]
        [CustomAuthorize(8, "ManageBooks")]
        public async Task<IActionResult> Delete(int bookId)
        {
            var existingUser = await _bookRepository.GetByID(bookId);
            if (existingUser == null)
            {
                return Ok(new Response
                {
                    Status = 0,
                    Message = "User not found."
                });
            }

            int rowsAffected = await _bookRepository.Delete(bookId);
            if (rowsAffected == 0)
            {
                return StatusCode(500, new Response
                {
                    Status = 0,
                    Message = "An error occurred while deleting the user."
                });
            }

            return Ok(new Response
            {
                Status = 1,
                Message = "Book deleted successfully."
            });
        }
    }
}
