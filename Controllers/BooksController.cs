using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace LibrarianWorkplaceAPI.Controllers
{
    [ApiController]
    [Route("books")]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly ApplicationContext _context;

        public BooksController(ILogger<BooksController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }


        // GET: 
        // Возвращает данные о книге по артикулу
        [HttpGet("bookbyid/{vendorCode}")]
        public async Task<IActionResult> GetBookById(int? vendorCode)
        {
            if (vendorCode is null) return BadRequest();
            BookModel? book = await _context.Books.FirstOrDefaultAsync(b => b.VendorCode == vendorCode);
            if (book is null) return NotFound();

            return Ok(book);
        }

        // GET: 
        //Возвращает данные о книге по названию
        [HttpGet("bookbytitle/{title}")]
        public async Task<IActionResult> GetBookByTitle(string title)
        {

            var books = await _context.Books.Where(b => EF.Functions.Like(b.Title, $"%{title}%")).ToArrayAsync();
            if (books is null || books.Count() == 0) return NotFound(title);

            return Ok(books);
        }

        // GET: 
        // Возвращает список доступных для выдачи книг
        [HttpGet("availablebooks")]
        public async Task<IActionResult> GetAvailableBooks()
        {

            var books = await _context.Books.Where(book => book.NumberOfCopies > book.Readers.Count || book.Readers == null).ToArrayAsync();
            if (books is null || books.Count() == 0) return Ok("All books are busy");

            return Ok(books);
        }

        // GET: 
        //Возвращает список выданных книг
        [HttpGet("givedbooks")]
        public async Task<IActionResult> GetGivedBooks()
        {

            var books = await _context.Books.Where(book => book.Readers != null && book.Readers.Count != 0).ToArrayAsync();
            if (books is null || books.Count() == 0) return Ok("All books are busy");

            return Ok(books);
        }


        // POST: 
        // Добавляет книгу
        [HttpPost("addbook")]
        public async Task<IActionResult> AddBook(BookGetModel book)
        {
            if (ModelState.IsValid) {
                BookModel newBook = new BookModel()
                {
                    Title = book.Title,
                    Author = book.Author,
                    ReleaseDate = book.ReleaseDate,
                    NumberOfCopies = book.NumberOfCopies
                };
                _context.Books.Add(newBook);
                await _context.SaveChangesAsync();
                return Ok(newBook);
            }
            return BadRequest(book);
        }

        // DELETE: 
        // Удаляет книгу
        [HttpDelete("deletebook/{vendorCode}")]
        public async Task<IActionResult> DeleteBook(int vendorCode)
        {
            var book = await _context.Books.FindAsync(vendorCode);
            if (book != null && _context.Books != null)
            {
                _context.Books.Attach(book);
                _context.Entry(book).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }

        // PUT: 
        // Меняет данные книги
        [HttpPut("changebook")]
        public async Task<IActionResult> ChangeBook(BookModel book)
        {
            if (ModelState.IsValid)
            {
                var bc = await _context.Books.FindAsync(book.VendorCode);
                if (bc != null && _context.Books != null)
                {
                    bc.Title = book.Title;
                    bc.Author = book.Author;
                    bc.ReleaseDate = book.ReleaseDate;
                    bc.NumberOfCopies = book.NumberOfCopies;
                    bc.Readers = book.Readers;

                    _context.Entry(bc).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(bc);
                }
                return NotFound(book);
            }
            return BadRequest(book);
        }

    }
}
