namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface IReadersRepository: IGenericRepository<ReaderModel>
    {
        Task<IEnumerable<ReaderModel>> GetReaderByName(string name);
        Task TakeBook(ReaderModel reader, BookModel book);
        Task ReturnBook(ReaderModel reader, BookModel book);
        void ChangeReader(ReaderModel reader);
    }
}
