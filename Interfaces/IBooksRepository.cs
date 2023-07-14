namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface IBooksRepository : IGenericRepository<BookModel>
    {
        IEnumerable<BookModel> GetByTitle(string title);
        IEnumerable<BookModel> GetAvailableBooks();
        IEnumerable<BookModel> GetGivedBooks();
        void ChangeBook(BookModel book);
    }
}
