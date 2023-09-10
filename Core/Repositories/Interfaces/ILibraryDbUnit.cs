namespace LibrarianWorkplaceAPI.Core.Repositories.Interfaces
{
    public interface ILibraryDbUnit : IDisposable
    {
        IBooksRepository Books { get; }
        IReadersRepository Readers { get; }
        Task<int> Commit();
    }
}
