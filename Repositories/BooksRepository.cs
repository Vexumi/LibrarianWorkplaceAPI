using LibrarianWorkplaceAPI.Interfaces;

namespace LibrarianWorkplaceAPI.Repositories
{
    public class BooksRepository : GenericRepository<BookModel>, IBooksRepository
    {
        public BooksRepository(ApplicationContext context) : base(context)
        {
        }

        public IEnumerable<BookModel> GetByTitle(string title)
        {
            return _context.Books.Where(d => d.Title == title).ToArray();
        }

        public IEnumerable<BookModel> GetAvailableBooks()
        {
            return _context.Books.Where(book => book.NumberOfCopies > book.Readers!.Count || book.Readers == null).ToArray();
        }

        public IEnumerable<BookModel> GetGivedBooks()
        {
            return _context.Books.Where(book => book.Readers != null && book.Readers.Count != 0).ToArray();
        }

        public void ChangeBook(BookModel book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
        }
    }
}
