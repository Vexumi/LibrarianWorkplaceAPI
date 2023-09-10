namespace LibrarianWorkplaceAPI.Core.Repositories.Interfaces
{
    public class UsersDbUnit: IUsersDbUnit
    {
        private readonly ApplicationContext _context;
        public IUsersRepository Users { get; set; }
        public UsersDbUnit(ApplicationContext context)
        {
            _context = context;
            Users = new UsersRepository(context);
        }

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
