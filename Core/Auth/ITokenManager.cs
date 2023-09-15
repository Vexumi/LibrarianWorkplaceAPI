namespace LibrarianWorkplaceAPI.Core.Auth
{
    public interface ITokenManager
    {
        string? Authenticate(string username, string password);
        bool? Register(UserModel user);
    }
}
