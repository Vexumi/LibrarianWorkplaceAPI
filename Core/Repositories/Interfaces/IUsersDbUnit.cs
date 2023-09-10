namespace LibrarianWorkplaceAPI.Core.Repositories.Interfaces
{
    public interface IUsersDbUnit : IDisposable
    {
        IUsersRepository Users { get; }
        Task<int> Commit();
    }
}
