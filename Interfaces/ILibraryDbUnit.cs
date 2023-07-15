namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface ILibraryDbUnit : IDisposable
    {
        IBooksRepository Books { get; }
        IReadersRepository Readers { get; }
        Task<int> Commit();
    }
}
