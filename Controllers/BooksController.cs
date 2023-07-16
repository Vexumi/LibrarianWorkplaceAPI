using LibrarianWorkplaceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

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


        /// <summary>
        /// Возвращает все книги
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookModel[]>> GetAllBooks()
        {
            return Ok((await _context.Books.GetAll()).ToArray());
        }

        /// <summary>
        /// Возвращает данные о книге по артикулу
        /// </summary>
        /// <param name="vendorCode">Артикул книги</param>
        /// <returns></returns>
        [HttpGet("{vendorCode:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookModel>> GetBookById(int vendorCode)
        {
            BookModel? book = await _context.Books.GetById(vendorCode);
            if (book is null) return NotFound();

            return Ok(book);
        }

        /// <summary>
        /// Возвращает данные о книге по названию
        /// </summary>
        /// <param name="title">Название книги</param>
        /// <returns></returns>
        [HttpGet("{title:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookModel[]>> GetBookByTitle(string title)
        {
            IEnumerable<BookModel>? books = await _context.Books.GetByTitle(title);
            if (books is null || !books.Any()) return NotFound();

            return Ok(books.ToArray());
        }

        /// <summary>
        /// Возвращает список доступных для выдачи книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookModel[]>> GetAvailableBooks()
        {
            IEnumerable<BookModel>? books = await _context.Books.GetAvailableBooks();
            if (books is null || !books.Any()) return Ok("All books are busy");

            return Ok(books.ToArray());
        }

        /// <summary>
        /// Возвращает список выданных книг
        /// </summary>
        /// <returns></returns>
        [HttpGet("gived")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookModel[]>> GetGivedBooks()
        {
            IEnumerable<BookModel>? books = await _context.Books.GetGivedBooks();
            if (books is null || !books.Any()) return Ok("All books are free");

            return Ok(books.ToArray());
        }


        /// <summary>
        /// Добавляет книгу
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBook([FromBody]BookGetModel book)
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
                return CreatedAtAction(nameof(GetBookById), new { vendorCode = newBook.VendorCode}, newBook);
            }
            return BadRequest();
        }

        /// <summary>
        /// Удаляет книгу
        /// </summary>
        /// <param name="vendorCode">Артикул книги</param>
        /// <returns></returns>
        [HttpDelete("{vendorCode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook([FromRoute]int vendorCode)
        {
            var book = await _context.Books.GetById(vendorCode);
            if (book == null) return NotFound();

            if (book.Readers != null && book.Readers.Count > 0)
            {
                while(book.Readers.Count > 0)
                {
                    await ReturnBook(book.Readers[0], book.VendorCode);
                }
            }

            _context.Books.Remove(book);
            await _context.Commit();
            return NoContent();
            
        }

        /// <summary>
        /// Редактирует данные книги
        /// </summary>
        /// <param name="vendorCode"></param>
        /// <param name="patchedBook"></param>
        /// <returns></returns>
        [HttpPatch("{vendorCode:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeBook([FromRoute]int vendorCode, [FromBody]BookPatchModel patchedBook)
        {
            if (!ModelState.IsValid) return BadRequest();

            var book = await _context.Books.GetById(vendorCode);
            if (book == null) return NotFound();

            book.Title = patchedBook.IsFieldPresent(nameof(book.Title)) ? patchedBook.Title : book.Title;
            book.Author = patchedBook.IsFieldPresent(nameof(book.Author)) ? patchedBook.Author : book.Author;
            book.ReleaseDate = patchedBook.IsFieldPresent(nameof(book.ReleaseDate)) && patchedBook.ReleaseDate != null ? (DateTime)patchedBook.ReleaseDate : book.ReleaseDate;
            book.NumberOfCopies = patchedBook.IsFieldPresent(nameof(book.NumberOfCopies)) && patchedBook.NumberOfCopies != null ? (int)patchedBook.NumberOfCopies : book.NumberOfCopies;

            _context.Books.Change(book);
            await _context.Commit();

            return NoContent();
        }


        /// <summary>
        /// Выдача книги читателю
        /// </summary>
        /// <param name="readerId">Id читателя</param>
        /// <param name="bookId">Id книги</param>
        /// <returns></returns>
        [HttpPut("take")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TakeBook(int readerId, int bookId)
        {
            var reader = _context.Readers.Find(r => r.Id == readerId).FirstOrDefault();
            var book = _context.Books.Find(r => r.VendorCode == bookId).FirstOrDefault();

            if (reader == null || book == null) return NotFound(reader == null ? "Reader" : "Book");

            if (book.Readers?.Count >= book.NumberOfCopies) return BadRequest("All books are busy");

            if (reader.Books != null && reader.Books.Contains(bookId)) return BadRequest("Reader has already taken this book!");

            await _context.Books.Take(reader, book);

            return NoContent();
        }

        /// <summary>
        /// Возврат книги в библиотеку
        /// </summary>
        /// <param name="readerId">Id читателя</param>
        /// <param name="vendorCode">Артикул книги</param>
        /// <returns></returns>
        [HttpPut("return")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnBook(int readerId, int vendorCode)
        {
            var reader = await _context.Readers.GetById(readerId);
            var book = await _context.Books.GetById(vendorCode);

            if (reader == null || book == null) return NotFound(reader == null ? "Reader" : "Book");

            if (reader.Books is null || !reader.Books.Contains(vendorCode)) return BadRequest("Reader didn't take the book");
            if (book.Readers is null || !book.Readers.Contains(readerId)) return BadRequest("Book was't issued to the reader");

            await _context.Books.Return(reader, book);

            return NoContent();
        }
    }
}
