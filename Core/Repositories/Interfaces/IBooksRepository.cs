namespace LibrarianWorkplaceAPI.Core.Repositories.Interfaces
{
    public interface IBooksRepository : IGenericRepository<BookModel>
    {
        Task<IEnumerable<BookModel>> GetByTitle(string title);
        Task<IEnumerable<BookModel>> GetAvailableBooks();
        Task<IEnumerable<BookModel>> GetGivedBooks();
        Task Take(ReaderModel reader, BookModel book);
        Task Return(ReaderModel reader, BookModel book);
    }
}
