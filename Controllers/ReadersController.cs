using LibrarianWorkplaceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection.PortableExecutable;

namespace LibrarianWorkplaceAPI.Controllers
{
    [ApiController]
    [Route("api/readers")]
    public class ReadersController : ControllerBase
    {
        private readonly ILogger<ReadersController> _logger;
        private readonly ILibraryDbUnit _context;

        public ReadersController(ILogger<ReadersController> logger, ILibraryDbUnit context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: /GetReaderById
        // Возвращает всех читателей
        [HttpGet("getallreaders")]
        public async Task<ActionResult<BookModel[]>> GetAllReaders()
        {
            return Ok((await _context.Readers.GetAll()).ToArray());
        }

        // GET: /GetReaderById
        // Возвращает информацию о читателе по id
        [HttpGet("readerbyid/{id}")]
        public async Task<ActionResult<ReaderModel>> GetReaderById(int id)
        {
            ReaderModel? reader = await _context.Readers.GetById(id);
            if (reader is null) return NotFound(id);

            return Ok(reader);
        }

        // GET: /GetReaderByName
        // Возвращает информацию о читателе по ФИО или отрывку из ФИО
        [HttpGet("readerbyname/{name}")]
        public async Task<ActionResult<ReaderModel>> GetReaderByName(string name)
        {
            var reader = (await _context.Readers.GetReaderByName(name)).ToArray();
            if (reader is null || reader.Count() == 0) return NotFound(name);

            return Ok(reader);
        }

        // POST: /AddReader
        // Добавляет читателя
        [HttpPost("addreader")]
        public IActionResult AddReader(ReaderGetModel reader)
        {
            if (ModelState.IsValid)
            {
                ReaderModel newReader = new ReaderModel()
                {
                    FullName = reader.FullName,
                    DateOfBirth = reader.DateOfBirth
                };
                _context.Readers.Add(newReader);
                _context.Commit();
                return Ok(newReader);
            }
            return BadRequest(reader);
        }

        // DELETE: /DeleteReader
        // Удаляет читателя по id
        [HttpDelete("deletereader/{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = _context.Readers.Find(r => r.Id == id).FirstOrDefault();
            if (reader != null && _context.Readers != null)
            {
                _context.Readers.Remove(reader);
                await _context.Commit();
                return Ok();
            }
            return NotFound();
        }

        // PUT: /ChangeReader
        // Меняет данные читателя 
        [HttpPut("changereader")]
        public async Task<ActionResult<ReaderModel>> ChangeReader(ReaderModel reader)
        {
            if (ModelState.IsValid)
            {
                var rd = _context.Readers.Find(r => r.Id == reader.Id).FirstOrDefault();
                if (rd != null)
                {
                    rd.FullName = reader.FullName;
                    rd.DateOfBirth = reader.DateOfBirth;
                    rd.Books = reader.Books;
                    _context.Readers.ChangeReader(rd);
                    await _context.Commit();
                    return Ok(rd);
                }
                return NotFound(reader);
            }
            return BadRequest(reader);
        }

        // POST: /TakeBook 
        // Выдача книга читателю
        [HttpPost("takebook")]
        public async Task<IActionResult> TakeBook(int readerId, int bookId)
        {
            var reader = _context.Readers.Find(r => r.Id == readerId).FirstOrDefault();
            var book = _context.Books.Find(r => r.VendorCode == bookId).FirstOrDefault();

            if (reader == null || book == null) return NotFound(reader == null ? "Reader" : "Book");

            if (book.Readers?.Count >= book.NumberOfCopies) return BadRequest("All books are busy");

            if (reader.Books != null && reader.Books.Contains(bookId)) return BadRequest("Reader already taked this book!");

            await _context.Readers.TakeBook(reader, book);

            return Ok();
        }

        // POST: /ReturnBook 
        // Возврат книги в библиотеку
        [HttpPost("returnbook")]
        public async Task<IActionResult> ReturnBook(int readerId, int bookId)
        {
            var reader = _context.Readers.Find(r => r.Id == readerId).FirstOrDefault();
            var book = _context.Books.Find(r => r.VendorCode == bookId).FirstOrDefault();

            if (reader == null || book == null) return NotFound(reader == null ? "Reader" : "Book");

            if (reader.Books is null || !reader.Books.Contains(bookId)) return BadRequest("Reader didn't take the book");
            if (book.Readers is null || !book.Readers.Contains(readerId)) return BadRequest("Book was't issued to the reader");

            await _context.Readers.ReturnBook(reader, book);

            return Ok();
        }
    }
}
