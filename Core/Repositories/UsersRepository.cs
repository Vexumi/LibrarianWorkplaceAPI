using LibrarianWorkplaceAPI.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace LibrarianWorkplaceAPI.Core.Repositories
{
    public class UsersRepository : GenericRepository<UserModel>, IUsersRepository
    {
        public UsersRepository(ApplicationContext context): base(context)
        {
            
        }
        public async Task<UserModel> GetByLibraryName(string libraryName)
        {
            return await _context.Users.Where(user => user.LibraryName == libraryName).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
