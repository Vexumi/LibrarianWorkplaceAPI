namespace LibrarianWorkplaceAPI.Interfaces
{
    public interface ILibraryDbUnit : IDisposable
    {
        IBooksRepository Books { get; }
        IReadersRepository Readers { get; }
        int Commit();
    }
}
