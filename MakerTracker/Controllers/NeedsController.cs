namespace MakerTracker.Controllers
{
    using System.Collections.Generic;
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
    ///     REST controller for manipulating requestor needs
    /// </summary>
    /// <seealso cref="MakerTracker.Controllers.ApiBaseController" />
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NeedsController : ApiBaseController
    {
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NeedController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The automapper.</param>
        public NeedsController(MakerTrackerContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets the entries from the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetNeeds()
        {
            var profile = await GetLoggedInProfile();
            return Ok(_context.Needs.ProjectTo<NeedDto>(_mapper.ConfigurationProvider));
        }

        /// <summary>
        ///     Gets the entry matching a specific identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Need>> GetNeed(int id)
        {
            var entry = await _context.Needs.FindAsync(id);
            var profile = await GetLoggedInProfile();
            if (entry == null || entry.ProfileId != profile.Id)
            {
                return NotFound();
            }
            return entry == null ? (ActionResult)NotFound() : Ok(entry);
        }

        /// <summary>
        ///     Updates the entry received via a PUT request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNeed(int id, NeedDto entry)
        {
            if (id != entry.Id)
            {
                return BadRequest("Payload does not match identifier");
            }

            var profile = await GetLoggedInProfile();
            var allowedProfiles = await GetAllowedProfiles(profile.Id);
            if (!await _context.Needs.AnyAsync(e => e.Id == entry.Id && allowedProfiles.Contains(e.ProfileId)))
            {
                return NotFound();
            }

            var dbEntry = _mapper.Map<Need>(entry);
            _context.Entry(dbEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NeedExists(id))
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
        ///     Creates the entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Need>> PostNeed(NeedDto entry)
        {
            var profile = await GetLoggedInProfile();
            var allowedProfiles = await GetAllowedProfiles(profile.Id);
            if (!VerifyProfileOnEntry(allowedProfiles, entry))
            {
                return Forbid("User does not have access to create needs for this account");
            }

            var dbEntry = SetUpNeed(entry, profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Post", new { id = entry.Id }, _mapper.Map<NeedDto>(dbEntry));
        }

        /// <summary>
        ///     Creates the entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [HttpPost("bulk")]
        public async Task<ActionResult<Need>> PostNeedBulk(NeedDto[] entries)
        {
            var profile = await GetLoggedInProfile();
            var allowedProfiles = await GetAllowedProfiles(profile.Id);
            if (entries.Any(entry => !VerifyProfileOnEntry(allowedProfiles, entry)))
            {
                return Forbid("User does not have access to create needs for this account");
            }

            var dbEntries = entries.Select(e => SetUpNeed(e, profile)).ToList();
            await _context.SaveChangesAsync();
            var ids = dbEntries.Select(e => e.Id).ToList();
            var results = _mapper.ProjectTo<NeedDto>(_context.Needs.Where(e => ids.Contains(e.Id)));
            return Created("api/Need/bulk", results);
        }

        /// <summary>
        ///     Deletes the entry matching the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Need>> DeleteNeed(int id)
        {
            var entry = await _context.Needs.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            var profile = await GetLoggedInProfile();
            if(profile.Id != entry.ProfileId)
            {
                return NotFound();
            }

            _context.Needs.Remove(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        private async Task<ICollection<int>> GetAllowedProfiles(int profileId)
        {
            var children = await _context.ProfileHierarchies.Where(e => e.ParentProfileId == profileId).Select(e => e.ChildProfileId).ToListAsync();
            children.Add(profileId);
            return children;
        }

        private bool NeedExists(int id)
        {
            return _context.Needs.Any(e => e.Id == id);
        }

        private bool VerifyProfileOnEntry(ICollection<int> allowedProfiles, NeedDto entry)
        {
            return entry.ProfileId <= 0 || allowedProfiles.Contains(entry.ProfileId);
        }

        private Need SetUpNeed(NeedDto entry, DBModels.Profile profile)
        {
            var dbEntry = _mapper.Map<Need>(entry);
            if (entry.ProfileId <= 0)
            {
                dbEntry.Profile = profile;
            }
            dbEntry.CreatedDate = System.DateTime.Now;
            _context.Needs.Add(dbEntry);
            return dbEntry;
        }
    }
}
