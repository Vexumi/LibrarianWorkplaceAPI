using LibrarianWorkplaceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LibrarianWorkplaceAPI.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly ILibraryDbUnit _context;

        public BooksController(ILogger<BooksController> logger, ILibraryDbUnit context)
        {
            _logger = logger;
            _context = context;
        }


        //GET
        // Возвращает все книги
        [HttpGet("getallbooks")]
        public async Task<ActionResult<BookModel[]>> GetAllBooks()
        {
            return Ok((await _context.Books.GetAll()).ToArray());
        }

        // GET: 
        // Возвращает данные о книге по артикулу
        [HttpGet("bookbyid/{vendorCode}")]
        public async Task<ActionResult<BookModel>> GetBookById(int vendorCode)
        {
            BookModel? book = await _context.Books.GetById(vendorCode);
            if (book is null) return NotFound();

            return Ok(book);
        }

        // GET: 
        //Возвращает данные о книге по названию
        [HttpGet("bookbytitle/{title}")]
        public async Task<ActionResult<BookModel[]>> GetBookByTitle(string title)
        {
            IEnumerable<BookModel>? books = await _context.Books.GetByTitle(title);
            if (books is null || books.Count() == 0) return NotFound();

            return Ok(books.ToArray());
        }

        // GET: 
        // Возвращает список доступных для выдачи книг
        [HttpGet("availablebooks")]
        public async Task<ActionResult<BookModel[]>> GetAvailableBooks()
        {

            IEnumerable<BookModel>? books = await _context.Books.GetAvailableBooks();
            if (books is null || books.Count() == 0) return Ok("All books are busy");

            return Ok(books.ToArray());
        }

        // GET: 
        //Возвращает список выданных книг
        [HttpGet("givedbooks")]
        public async Task<ActionResult<BookModel[]>> GetGivedBooks()
        {

            IEnumerable<BookModel>? books = await _context.Books.GetGivedBooks();
            if (books is null || books.Count() == 0) return Ok("All books are free");

            return Ok(books.ToArray());
        }


        // POST: 
        // Добавляет книгу
        [HttpPost("addbook")]
        public async Task<IActionResult> AddBook(BookGetModel book)
        {
            if (ModelState.IsValid) {
                var newBook = new BookModel()
                {
                    Title = book.Title,
                    Author = book.Author,
                    ReleaseDate = book.ReleaseDate,
                    NumberOfCopies = book.NumberOfCopies,
                };
                _context.Books.Add(newBook);
                await _context.Commit();
                return Ok();
            }
            return BadRequest();
        }

        // DELETE: 
        // Удаляет книгу
        [HttpDelete("deletebook/{vendorCode}")]
        public async Task<IActionResult> DeleteBook(int vendorCode)
        {
            var book = await _context.Books.GetById(vendorCode);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.Commit();
                return Ok();
            }
            return NotFound();
        }

        // PUT: 
        // Редактирует данные книги
        [HttpPut("changebook")]
        public async Task<IActionResult> ChangeBook(BookModel book)
        {
            if (ModelState.IsValid)
            {
                var bc = await _context.Books.GetById(book.VendorCode);
                if (bc != null && _context.Books.GetAll() != null)
                {
                    bc.Title = book.Title;
                    bc.Author = book.Author;
                    bc.ReleaseDate = book.ReleaseDate;
                    bc.NumberOfCopies = book.NumberOfCopies;
                    bc.Readers = book.Readers;

                    _context.Books.ChangeBook(bc);
                    await _context.Commit();
                    return Ok(bc);
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}
