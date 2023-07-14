using LibrarianWorkplaceAPI.Interfaces;

namespace LibrarianWorkplaceAPI.Repositories
{
    public class LibraryDbUnit: ILibraryDbUnit
    {
        private readonly ApplicationContext _context;
        public IBooksRepository Books { get; private set; }
        public IReadersRepository Readers { get; private set; }

        public LibraryDbUnit (ApplicationContext context)
        {
            _context = context;
            Books = new BooksRepository(_context);
            Readers = new ReadersRepository(_context);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
