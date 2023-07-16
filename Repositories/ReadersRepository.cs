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

        public async Task TakeBook(ReaderModel reader, BookModel book)
        {
            if (reader.Books != null) reader.Books.Add(book.VendorCode);
            else reader.Books = new List<int> { book.VendorCode };

            if (book.Readers != null) book.Readers.Add(reader.Id);
            else book.Readers = new List<int> { reader.Id };

            // _context.Update()
            _context.Entry(reader).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task ReturnBook(ReaderModel reader, BookModel book)
        {
            reader!.Books!.Remove(book.VendorCode);
            book!.Readers!.Remove(reader.Id);

            _context.Entry(reader).State = EntityState.Modified;
            _context.Entry(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task ChangeReader(ReaderModel reader)
        {
            _context.Readers.Update(reader);
            await _context.SaveChangesAsync();
        }
    }
}
