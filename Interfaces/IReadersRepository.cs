namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface IReadersRepository: IGenericRepository<ReaderModel>
    {
        IEnumerable<ReaderModel> GetReaderByName(string name);
        void TakeBook(ReaderModel reader, BookModel book);
        void ReturnBook(ReaderModel reader, BookModel book);
        void ChangeReader(ReaderModel reader);
    }
}
