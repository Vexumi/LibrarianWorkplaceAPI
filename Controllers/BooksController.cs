using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http;

namespace LibrarianWorkplaceAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly ApplicationContext _context;

        public BooksController(ILogger<BooksController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }


        // GET: /GetBookById?id=1
        [HttpGet(Name = "GetBookById")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookModel))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBookById(int? vendorCode)
        {
            if (vendorCode is null) return BadRequest();
            BookModel? book = await _context.Books.FirstOrDefaultAsync(b => b.VendorCode == vendorCode);
            if (book is null) return NotFound();

            return Ok(book);
        }


        // POST: /AddBook {BookModel book}
        [HttpPost(Name = "AddBook")]
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

        // DELETE: /DeleteBook?vendorCode=1
        [HttpDelete(Name = "DeleteBook")]
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

        // PUT: /ChangeBook {BookModel book}
        [HttpPut(Name = "ChangeBook")]
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
