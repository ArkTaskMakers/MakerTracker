namespace MakerTracker.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MakerTracker.DBModels;
    using MakerTracker.Models.Equipment;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    ///     REST controller for manipulating equipment
    /// </summary>
    /// <seealso cref="MakerTracker.Controllers.ApiBaseController" />
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ApiBaseController
    {
        private readonly IMapper _mapper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EquipmentController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The automapper.</param>
        public EquipmentController(MakerTrackerContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        /// <summary>
        ///     Gets the equipment entries from the database.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetEquipments()
        {
            return Ok(_context.Equipments.ProjectTo<EquipmentDto>(_mapper.ConfigurationProvider));
        }

        /// <summary>
        ///     Gets the equipment matching a specific identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var Equipment = await _context.Equipments.FindAsync(id);
            return Equipment == null ? (ActionResult)NotFound() : Ok(Equipment);
        }

        /// <summary>
        ///     Updates the equipment entry received via a PUT request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="entry">The entry.</param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, EquipmentDto entry)
        {
            if (id != entry.Id)
            {
                return BadRequest("Payload does not match identifier");
            }

            var dbEntry = _mapper.Map<Equipment>(entry);
            _context.Entry(dbEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
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
        ///     Creates the equipment entry received via a POST request.
        /// </summary>
        /// <param name="entry">The entry to create.</param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(EquipmentDto entry)
        {
            var dbEntry = _mapper.Map<Equipment>(entry);
            _context.Equipments.Add(dbEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Post", new { id = entry.Id }, entry);
        }

        /// <summary>
        ///     Deletes the equipment matching the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorize(Roles="Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Equipment>> DeleteEquipment(int id)
        {
            var entry = await _context.Equipments.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.Equipments.Remove(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.Id == id);
        }
    }
}
