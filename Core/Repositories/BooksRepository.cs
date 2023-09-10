using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarianWorkplaceAPI.Core.Repositories
{
    public class BooksRepository : GenericRepository<BookModel>, IBooksRepository
    {
        public BooksRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BookModel>> GetByTitle(string title)
        {
            return await _context.Books.Where(d => d.Title == title).ToArrayAsync();
        }

        public async Task<IEnumerable<BookModel>> GetAvailableBooks()
        {
            return await _context.Books.Where(book => book.NumberOfCopies > book.Readers!.Count || book.Readers == null).ToArrayAsync();
        }

        public async Task<IEnumerable<BookModel>> GetGivedBooks()
        {
            return await _context.Books.Where(book => book.Readers != null && book.Readers.Count != 0).ToArrayAsync();
        }

        public async Task Take(ReaderModel reader, BookModel book)
        {
            if (reader.Books != null) reader.Books.Add(book.VendorCode);
            else reader.Books = new List<int> { book.VendorCode };

            if (book.Readers != null) book.Readers.Add(reader.Id);
            else book.Readers = new List<int> { reader.Id };

            // _context.Update()
            _context.Entry(reader).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Return(ReaderModel reader, BookModel book)
        {
            reader!.Books!.Remove(book.VendorCode);
            book!.Readers!.Remove(reader.Id);

            _context.Entry(reader).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
