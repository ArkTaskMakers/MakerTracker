namespace MakerTracker.Controllers
{
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
    ///     REST controller for manipulating maker equipment
    /// </summary>
    /// <seealso cref="MakerTracker.Controllers.ApiBaseController" />
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MakerEquipmentController : ApiBaseController
    {
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MakerEquipmentController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The automapper.</param>
        public MakerEquipmentController(MakerTrackerContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets the maker equipment entries from the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMakerEquipments()
        {
            var profile = await GetLoggedInProfile();
            return Ok(_context.MakerEquipment.ProjectTo<MakerEquipmentDto>(_mapper.ConfigurationProvider));
        }

        /// <summary>
        ///     Gets the maker equipment matching a specific identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<MakerEquipment>> GetMakerEquipment(int id)
        {
            var entry = await _context.MakerEquipment.FindAsync(id);
            var profile = await GetLoggedInProfile();
            if (entry == null || entry.ProfileId != profile.Id)
            {
                return NotFound();
            }
            return entry == null ? (ActionResult)NotFound() : Ok(entry);
        }

        /// <summary>
        ///     Updates the maker equipment entry received via a PUT request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMakerEquipment(int id, MakerEquipmentDto entry)
        {
            if (id != entry.Id)
            {
                return BadRequest("Payload does not match identifier");
            }

            var profile = await GetLoggedInProfile();
            if (!await _context.MakerEquipment.AnyAsync(e => e.ProfileId == profile.Id && e.Id == entry.Id))
            {
                return NotFound();
            }

            var dbEntry = _mapper.Map<MakerEquipment>(entry);
            _context.Entry(dbEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MakerEquipmentExists(id))
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
        ///     Creates the maker Equipment entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<MakerEquipment>> PostMakerEquipment(MakerEquipmentDto entry)
        {
            var profile = await GetLoggedInProfile();
            var dbEntry = SetUpMakerEquipment(entry, profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Post", new { id = entry.Id }, _mapper.Map<MakerEquipmentDto>(dbEntry));
        }

        /// <summary>
        ///     Creates the maker equipment entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [HttpPost("bulk")]
        public async Task<ActionResult<MakerEquipment>> PostMakerEquipmentBulk(MakerEquipmentDto[] entries)
        {
            var profile = await GetLoggedInProfile();
            var dbEntries = entries.Select(e => SetUpMakerEquipment(e, profile)).ToList();
            await _context.SaveChangesAsync();
            var ids = dbEntries.Select(e => e.Id).ToList();
            var results = _mapper.ProjectTo<MakerEquipmentDto>(_context.MakerEquipment.Where(e => ids.Contains(e.Id)));
            return Created("api/MakerEquipment/bulk", results);
        }

        private MakerEquipment SetUpMakerEquipment(MakerEquipmentDto entry, DBModels.Profile profile)
        {
            var dbEntry = _mapper.Map<MakerEquipment>(entry);
            dbEntry.Profile = profile;
            _context.MakerEquipment.Add(dbEntry);
            return dbEntry;
        }

        /// <summary>
        ///     Deletes the maker equipment matching the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<MakerEquipment>> DeleteMakerEquipment(int id)
        {
            var entry = await _context.MakerEquipment.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            var profile = await GetLoggedInProfile();
            if(profile.Id != entry.ProfileId)
            {
                return NotFound();
            }

            _context.MakerEquipment.Remove(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        private bool MakerEquipmentExists(int id)
        {
            return _context.MakerEquipment.Any(e => e.Id == id);
        }
    }
}
