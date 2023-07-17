using LibrarianWorkplaceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarianWorkplaceAPI.Controllers
{
    [ApiController]
    [Route("api/readers")]
    public class ReadersController : ControllerBase
    {
        private readonly ILogger<ReadersController> _logger;
        private readonly ILibraryDbUnit _context;

        public ReadersController(ILogger<ReadersController> logger, ILibraryDbUnit context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Возвращает всех читателей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookModel[]>> GetAllReaders()
        {
            return Ok((await _context.Readers.GetAll()).ToArray());
        }

        /// <summary>
        /// Возвращает информацию о читателе по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReaderModel>> GetReaderById(int id)
        {
            ReaderModel? reader = await _context.Readers.GetById(id);
            if (reader is null) return NotFound();

            return Ok(reader);
        }

        /// <summary>
        /// Возвращает информацию о читателе по ФИО или отрывку из ФИО
        /// </summary>
        /// <param name="name">ФИО читателя</param>
        /// <returns></returns>
        [HttpGet("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReaderModel>> GetReaderByName(string name)
        {
            var reader = (await _context.Readers.GetReaderByName(name)).ToArray();
            if (reader is null || reader.Count() == 0) return NotFound();

            return Ok(reader);
        }

        /// <summary>
        /// Добавляет читателя
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddReader(ReaderGetModel reader)
        {
            if (ModelState.IsValid)
            {
                ReaderModel newReader = new ReaderModel()
                {
                    FullName = reader.FullName,
                    DateOfBirth = reader.DateOfBirth
                };
                _context.Readers.Add(newReader);
                await _context.Commit();
                return CreatedAtAction(nameof(GetReaderById), new { id = newReader.Id }, newReader);
            }
            return BadRequest();
        }

        /// <summary>
        /// Удаляет читателя по id
        /// </summary>
        /// <param name="id">Id читателя</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Readers.GetById(id);
            if (reader == null) return NotFound();
            
            if (reader.Books != null && reader.Books.Any()) return BadRequest("Not all books returned");

            _context.Readers.Remove(reader);
            await _context.Commit();
            return NoContent();
        }

        /// <summary>
        /// Редактирует данные читателя 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchedReader"></param>
        /// <returns></returns>
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeReader([FromRoute]int id, [FromBody]ReaderPatchModel patchedReader)
        {
            if (!ModelState.IsValid) return BadRequest();

            var reader = await _context.Readers.GetById(id);
            if (reader == null) return NotFound();
            
            reader.FullName = patchedReader.IsFieldPresent(nameof(reader.FullName)) ? patchedReader.FullName : reader.FullName;
            reader.DateOfBirth = patchedReader.IsFieldPresent(nameof(reader.DateOfBirth)) ? patchedReader.DateOfBirth : reader.DateOfBirth;

            _context.Readers.Change(reader);
            await _context.Commit();

            return NoContent();
        }
    }
}
