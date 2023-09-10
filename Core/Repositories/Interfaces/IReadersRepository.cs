namespace LibrarianWorkplaceAPI.Core.Repositories.Interfaces
{
    public interface IReadersRepository : IGenericRepository<ReaderModel>
    {
        Task<IEnumerable<ReaderModel>> GetReaderByName(string name);
    }
}
