using LibrarianWorkplaceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Runtime.CompilerServices;

namespace LibrarianWorkplaceAPI.Repositories
{
    public class ReadersRepository: GenericRepository<ReaderModel>, IReadersRepository
    {
        public ReadersRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReaderModel>> GetReaderByName(string name)
        {
            return await _context.Readers.Where(r => EF.Functions.Like(r.FullName, $"%{name}%")).ToArrayAsync();
        }
    }
}
