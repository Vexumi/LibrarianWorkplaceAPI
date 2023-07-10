using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarianWorkplaceAPI
{

    public interface ILibrarianWorkplaceService
    {

        // Books
        IEnumerable<BookModel> GetBooks();
        void AddBook(BookGetModel book);
        Task<BookModel?> GetBookById(int vendorCode);
        Task<BookModel[]?> GetBooksByTitle(string title);
        Task<ObjectResult> RemoveBook(int vendorCode);

        // Readers
        IEnumerable<ReaderModel> GetReaders();
        void AddReader(ReaderGetModel reader);
        Task<ReaderModel?> GetReaderById(int id);
        Task<ReaderModel[]?> GetReadersByName(string name);
        Task<ObjectResult> RemoveReader(int id);

        // Other
        void SaveChangesAsync();
    }

    public class LibrarianWorkplaceService : ILibrarianWorkplaceService
    {
        private readonly ApplicationContext _context;

        public LibrarianWorkplaceService(ApplicationContext context)
        {
            this._context = context;
        }

        public IEnumerable<BookModel> GetBooks()
        {
            return this._context.Books;
        }

        public async void AddBook(BookGetModel book)
        {
            BookModel newBook = new BookModel()
            {
                Title = book.Title,
                Author = book.Author,
                ReleaseDate = book.ReleaseDate,
                NumberOfCopies = book.NumberOfCopies
            };
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
        }

        public async Task<ObjectResult> RemoveBook(int vendorCode)
        {
            var book = await _context.Books.FindAsync(vendorCode);
            if (book != null && _context.Books != null)
            {
                _context.Books.Attach(book);
                _context.Entry(book).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return new OkObjectResult(book);
            }
            return new NotFoundObjectResult(vendorCode);
        }

        public async Task<BookModel?> GetBookById(int vendorCode)
        {
            BookModel? book = await _context.Books.FirstOrDefaultAsync(b => b.VendorCode == vendorCode);
            if (book is null) return null;

            return book;
        }

        public async Task<BookModel[]?> GetBooksByTitle(string title)
        {
            var books = await _context.Books.Where(b => EF.Functions.Like(b.Title, $"%{title}%")).ToArrayAsync();
            if (books is null || books.Count() == 0) return null;

            return books;
        }

        public IEnumerable<ReaderModel> GetReaders()
        {
            return _context.Readers;
        }

        public async void AddReader(ReaderGetModel reader)
        {
            ReaderModel newReader = new ReaderModel()
            {
                FullName = reader.FullName,
                DateOfBirth = reader.DateOfBirth
            };
            _context.Readers.Add(newReader);
            await _context.SaveChangesAsync();
        }

        public async Task<ObjectResult> RemoveReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader != null && _context.Readers != null)
            {
                _context.Readers.Attach(reader);
                _context.Entry(reader).State = EntityState.Deleted;
                await _context.SaveChangesAsync();

                return new OkObjectResult(reader);
            }
            return new NotFoundObjectResult(id);
        }

        public async Task<ReaderModel[]?> GetReadersByName(string name)
        {
            var readers = await _context.Readers.Where(r => EF.Functions.Like(r.FullName, $"%{name}%")).ToArrayAsync();
            if (readers is null || readers.Count() == 0) return null;

            return readers;
        }

        public async Task<ReaderModel?> GetReaderById(int id)
        {
            ReaderModel? readers = await _context.Readers.FirstOrDefaultAsync(r => r.Id == id);
            if (readers is null) return null;

            return readers;
        }

        public async void SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
