using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace LibrarianWorkplaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReadersController : ControllerBase
    {
        private readonly ILogger<ReadersController> _logger;
        private readonly ApplicationContext _context;

        public ReadersController(ILogger<ReadersController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /GetReaderById
        // Возвращает информацию о читателе по id
        [HttpGet(Name = "GetReaderById")]
        public async Task<IActionResult> GetReaderById(int id)
        {
            ReaderModel? reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == id);
            if (reader is null) return NotFound(id);

            return Ok(reader);
        }

        // GET: /GetReaderByName
        // Возвращает информацию о читателе по ФИО или отрывку из ФИО
        [HttpGet(Name = "GetReaderByName")]
        public async Task<IActionResult> GetReaderByName(string name)
        {

            var reader = await _context.Readers.Where(r => EF.Functions.Like(r.FullName, $"%{name}%")).ToArrayAsync();
            if (reader is null || reader.Count() == 0) return NotFound(name);

            return Ok(reader);
        }

        // POST: /AddReader
        // Добавляет читателя
        [HttpPost(Name = "AddReader")]
        public async Task<IActionResult> AddReader(ReaderGetModel reader)
        {
            if (ModelState.IsValid)
            {
                ReaderModel newReader = new ReaderModel()
                {
                    FullName = reader.FullName,
                    DateOfBirth = reader.DateOfBirth
                };
                _context.Readers.Add(newReader);
                await _context.SaveChangesAsync();
                return Ok(newReader);
            }
            return BadRequest(reader);
        }

        // DELETE: /DeleteReader
        // Удаляет читателя по id
        [HttpDelete(Name = "DeleteReader")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader != null && _context.Readers != null)
            {
                _context.Readers.Attach(reader);
                _context.Entry(reader).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return Ok();
            }
            return NotFound();
        }

        // PUT: /ChangeReader
        // Меняет данные читателя 
        [HttpPut(Name = "ChangeReader")]
        public async Task<IActionResult> ChangeReader(ReaderModel reader)
        {
            if (ModelState.IsValid)
            {
                var rd = await _context.Readers.FindAsync(reader.Id);
                if (rd != null)
                {
                    rd.FullName = reader.FullName;
                    rd.DateOfBirth = reader.DateOfBirth;
                    rd.Books = reader.Books;
                    _context.Entry(rd).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return Ok(rd);
                }
                return NotFound(reader);
            }
            return BadRequest(reader);
        }

        // POST: /TakeBook 
        // Выдача книга читателю
        [HttpPost(Name = "TakeBook")]
        public async Task<IActionResult> TakeBook(int readerId, int bookId)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == readerId);
            var book = await _context.Books.FirstOrDefaultAsync(r => r.VendorCode == bookId);

            if (reader == null || book == null) return NotFound(reader == null ? "Reader" : "Book");

            if (book.Readers?.Count >= book.NumberOfCopies) return BadRequest("All books are busy");

            if (reader.Books != null && reader.Books.Contains(bookId)) return BadRequest("Reader already taked this book!");

            if (reader.Books != null) reader.Books.Add(bookId);
            else reader.Books = new List<int> { bookId };

            if (book.Readers != null) book.Readers.Add(readerId);
            else book.Readers = new List<int> { readerId };

            _context.Entry(reader).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(reader);
        }

        // POST: /ReturnBook 
        // Возврат книги в библиотеку
        [HttpPost(Name = "ReturnBook")]
        public async Task<IActionResult> ReturnBook(int readerId, int bookId)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == readerId);
            var book = await _context.Books.FirstOrDefaultAsync(r => r.VendorCode == bookId);

            if (reader == null || book == null) return NotFound(reader == null ? "Reader" : "Book");

            if (!reader.Books.Contains(bookId)) return BadRequest("Reader didn't take the book");
            if (!book.Readers.Contains(readerId)) return BadRequest("Book was't issued to the reader");

            reader.Books.Remove(bookId);
            book.Readers.Remove(readerId);

            _context.Entry(reader).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(reader);
        }
    }
}
