namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface IBooksRepository : IGenericRepository<BookModel>
    {
        Task<IEnumerable<BookModel>> GetByTitle(string title);
        Task<IEnumerable<BookModel>> GetAvailableBooks();
        Task<IEnumerable<BookModel>> GetGivedBooks();
        Task ChangeBook(BookModel book);
    }
}
