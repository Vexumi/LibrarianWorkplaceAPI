namespace LibrarianWorkplaceAPI.Core.Repositories.Interfaces
{
    public interface IUsersRepository: IGenericRepository<UserModel>
    {
        Task<UserModel> GetByLibraryName(string libraryName);
    }
}
