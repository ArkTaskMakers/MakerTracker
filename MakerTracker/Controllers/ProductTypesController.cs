namespace MakerTracker.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using MakerTracker.DBModels;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ProductTypesController : ApiBaseController
    {
        private readonly IMapper _mapper;

        public ProductTypesController(MakerTrackerContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        // GET: api/ProductTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductTypeDto>>> GetProductTypes()
        {
            return await _context.ProductTypes.ProjectTo<ProductTypeDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // GET: api/ProductTypes
        [HttpGet("Query")]
        [EnableQuery]
        public IActionResult QueryProductTypes()
        {
            return Ok(_context.ProductTypes);
        }

        // GET: api/ProductTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductType>> GetProductType(int id)
        {
            var product = await _context.ProductTypes.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/ProductTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles="Admin")]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProductType(int id, ProductType product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<ActionResult<ProductType>> PostProduct(ProductType entry)
        {
            _context.ProductTypes.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = entry.Id }, entry);
        }

        // DELETE: api/ProductTypes/5
        [Authorize(Roles="Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductType>> DeleteProductType(int id)
        {
            var entry = await _context.ProductTypes.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.ProductTypes.Remove(entry);
            await _context.SaveChangesAsync();

            return entry;
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.Id == id);
        }
    }
}
