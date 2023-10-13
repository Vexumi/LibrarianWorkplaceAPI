using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;

namespace LibrarianWorkplaceAPI.Core.Repositories
{
    public class LibraryDbUnit : ILibraryDbUnit
    {
        private readonly ApplicationContext _context;
        public IBooksRepository Books { get; private set; }
        public IReadersRepository Readers { get; private set; }
        public IUsersRepository Users { get; private set; }

        public LibraryDbUnit(ApplicationContext context)
        {
            _context = context;
            Books = new BooksRepository(_context);
            Readers = new ReadersRepository(_context);
            Users = new UsersRepository(_context);
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
