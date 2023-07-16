namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface IReadersRepository: IGenericRepository<ReaderModel>
    {
        Task<IEnumerable<ReaderModel>> GetReaderByName(string name);
    }
}
