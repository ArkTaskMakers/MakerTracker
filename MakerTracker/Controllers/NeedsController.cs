namespace MakerTracker.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MakerTracker.DBModels;
    using MakerTracker.Models;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Profile = DBModels.Profile;

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
        [EnableQuery]
        public async Task<IActionResult> GetNeeds()
        {
            var profile = await GetLoggedInProfile();
            return Ok(_context.Needs.Where(e => e.ProfileId == profile.Id).Include(e => e.Transactions).ProjectTo<NeedDto>(_mapper.ConfigurationProvider));
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
        ///     Gets the outstanding needs for a specific product
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("products/{productId}")]
        [EnableQuery]
        public async Task<ActionResult<IQueryable<NeedLookupDto>>> GetOutstandingNeedsLookup(int productId)
        {
            var results = await _context.Needs.Where(e => e.FulfilledDate == null && e.ProductId == productId && e.Quantity > 0)
                .Select(e => new NeedLookupDto
                {
                    NeedId = e.Id,
                    DueDate = e.DueDate,
                    OutstandingQuantity = e.Transactions.Any() ? e.Quantity - e.Transactions.Sum(e => e.Amount) : e.Quantity,
                    ProductId = e.ProductId,
                    ProfileDisplayName = e.Profile.CompanyName ?? $"{e.Profile.FirstName} {e.Profile.LastName}"
                }).Where(e => e.OutstandingQuantity > 0)
                .ToListAsync();

            return Ok(results.AsQueryable());
        }

        /// <summary>
        ///     Gets the overall stats for all outstanding needs.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("summary/all")]
        public async Task<ActionResult<AllNeedsSummaryDto>> GetNeedSummaryAll()
        {
            var needsMet = _context.Needs.Where(e => e.FulfilledDate != null).Sum(e => e.Quantity);
            var outstandingNeeds = await _context.Needs.Where(e => e.FulfilledDate == null)
                .GroupJoin(_context.Transactions, e => e.Id, e => e.NeedId, (need, transactions) => need.Quantity - transactions.Sum(t => t.Amount))
                .SumAsync(needBalance => needBalance < 0 ? 0 : needBalance);

            return Ok(new AllNeedsSummaryDto
            {
                NeedsMet = needsMet,
                OutstandingNeeds = outstandingNeeds
            });
        }

        [HttpPost("fulfill/{id}")]
        public async Task<IActionResult> Fulfill(int id)
        {
            var entry = await _context.Needs.FindAsync(id);
            var profile = await GetLoggedInProfile();
            if (entry == null || entry.ProfileId != profile.Id)
            {
                return NotFound();
            }

            entry.FulfilledDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok();
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

            // Delete the entries not posted.
            var ids = entries.Select(e => e.Id).Where(e => e > 0).ToList();
            ProcessDeletions(_context.Needs.Where(e => e.ProfileId == profile.Id && !ids.Contains(e.Id) && e.FulfilledDate == null), profile);
            
            // Create/Update the posted records.
            var dbEntries = entries.Select(e => SetUpNeed(e, profile)).ToList();
            await _context.SaveChangesAsync();
            var results = _mapper.ProjectTo<NeedDto>(_context.Needs.Where(e => e.ProfileId == profile.Id));
            return Created("api/Need/bulk", results);
        }

        /// <summary>
        ///     Processes the entries that need to be deleted. If there are existing transactions attributed to the Need it
        ///     modifies the quantity and considers it fulfilled.
        /// </summary>
        /// <param name="ids">The ids to delete.</param>
        /// <param name="profile">The profile.</param>
        protected void ProcessDeletions(IQueryable<Need> needs, Profile profile)
        {
            // No transactions? Simple deletion.
            var toDelete = needs.Where(e => !e.Transactions.Any());
            _context.Needs.RemoveRange(toDelete);

            // Transactions? Cap the quantity to the transaction total and mark fulfilled.
            var toFulfill = needs.Where(e => e.Transactions.Any());
            foreach (var entry in toFulfill)
            {
                entry.Quantity = entry.Transactions.Sum(e => e.Amount);
                entry.FulfilledDate = DateTime.Now;
                _context.Entry(entry).State = EntityState.Modified;
            }
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
            if (profile.Id != entry.ProfileId)
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

        private Need SetUpNeed(NeedDto entry, Profile profile)
        {
            var dbEntry = _mapper.Map<Need>(entry);
            if (entry.ProfileId <= 0)
            {
                dbEntry.Profile = profile;
            }

            if (dbEntry.Id > 0)
            {
                _context.Entry(dbEntry).State = EntityState.Modified;
            }
            else
            {
                dbEntry.CreatedDate = System.DateTime.Now;
                _context.Needs.Add(dbEntry);
            }
            return dbEntry;
        }
    }
}
