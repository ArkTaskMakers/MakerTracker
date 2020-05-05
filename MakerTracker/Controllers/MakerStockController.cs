namespace MakerTracker.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MakerTracker.DBModels;
    using MakerTracker.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     REST controller for manipulating maker stock
    /// </summary>
    /// <seealso cref="MakerTracker.Controllers.ApiBaseController" />
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MakerStockController : ApiBaseController
    {
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MakerStockController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The automapper.</param>
        public MakerStockController(MakerTrackerContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets the maker stock entries from the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMakerStocks()
        {
            var profile = await GetLoggedInProfile();
            return Ok(_context.MakerStock.ProjectTo<MakerStockDto>(_mapper.ConfigurationProvider));
        }

        /// <summary>
        ///     Gets the maker stock matching a specific identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MakerStock>> GetMakerStock(int id)
        {
            var entry = await _context.MakerStock.FindAsync(id);
            int makerId = entry?.MakerId ?? 0;
            var profile = await GetLoggedInProfile();
            if (!await _context.Makers.Where(e => e.OwnerProfile.Id == profile.Id && e.Id == makerId).AnyAsync())
            {
                return NotFound();
            }
            return entry == null ? (ActionResult)NotFound() : Ok(entry);
        }

        /// <summary>
        ///     Updates the maker stock entry received via a PUT request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMakerStock(int id, MakerStockDto entry)
        {
            if (id != entry.Id)
            {
                return BadRequest("Payload does not match identifier");
            }

            var profile = await GetLoggedInProfile();
            if (!await _context.Makers.Where(e => e.OwnerProfile.Id == profile.Id && e.Id == entry.MakerId).AnyAsync())
            {
                return NotFound();
            }

            var dbEntry = _mapper.Map<MakerStock>(entry);
            _context.Entry(dbEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MakerStockExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        /// <summary>
        ///     Creates the maker stock entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MakerStock>> PostMakerStock(MakerStockDto entry)
        {
            var profile = await GetLoggedInProfile();
            var maker = await _context.Makers.Where(e => e.OwnerProfile.Id == profile.Id).SingleOrDefaultAsync();
            if (maker == null)
            {
                maker = new Maker
                {
                    OwnerProfile = profile
                };
                _context.Makers.Add(maker);
                await _context.SaveChangesAsync();
            }
            var dbEntry = _mapper.Map<MakerStock>(entry);
            entry.MakerId = maker.Id;
            entry.UpdatedOn = DateTime.Now;
            _context.MakerStock.Add(dbEntry);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Post", new { id = entry.Id }, entry);
        }

        /// <summary>
        ///     Creates the maker stock entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [HttpPost("bulk")]
        public async Task<ActionResult<MakerStock>> PostMakerStockBulk(MakerStockDto[] entries)
        {
            var maker = await EnsureMaker();
            var stock = entries.Select(e => SetUpMakerStock(e, maker)).ToList();
            await _context.SaveChangesAsync();
            return Created("api/MakerStock/bulk", stock);
        }

        private async Task<Maker> EnsureMaker()
        {
            var profile = await GetLoggedInProfile();
            var maker = await _context.Makers.Where(e => e.OwnerProfile.Id == profile.Id).SingleOrDefaultAsync();
            if (maker == null)
            {
                maker = new Maker
                {
                    OwnerProfile = profile
                };
                _context.Makers.Add(maker);
                await _context.SaveChangesAsync();
            }

            return maker;
        }

        private MakerStock SetUpMakerStock(MakerStockDto entry, Maker maker)
        {
            var dbEntry = _mapper.Map<MakerStock>(entry);
            dbEntry.Maker = maker;
            dbEntry.UpdatedOn = DateTime.Now;
            _context.MakerStock.Add(dbEntry);
            return dbEntry;
        }

        /// <summary>
        ///     Deletes the maker stock matching the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MakerStock>> DeleteMakerStock(int id)
        {
            var entry = await _context.MakerStock.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            var profile = await GetLoggedInProfile();
            if(!await _context.Makers.Where(e => e.OwnerProfile.Id == profile.Id && e.Id == entry.MakerId).AnyAsync())
            {
                return NotFound();
            }

            _context.MakerStock.Remove(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        private bool MakerStockExists(int id)
        {
            return _context.MakerStock.Any(e => e.Id == id);
        }
    }
}
