using LibrarianWorkplaceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibrarianWorkplaceAPI.Repositories
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

        public async void ChangeBook(BookModel book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
