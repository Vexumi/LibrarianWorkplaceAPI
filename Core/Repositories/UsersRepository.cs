using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;

namespace LibrarianWorkplaceAPI.Core.Repositories
{
    public class UsersRepository : GenericRepository<UserModel>, IUsersRepository
    {
        public UsersRepository(ApplicationContext context): base(context)
        {
            
        }
    }
}
