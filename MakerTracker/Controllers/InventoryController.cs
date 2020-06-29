namespace MakerTracker.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MakerTracker.DBModels;
    using MakerTracker.Models.Inventory;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ApiBaseController
    {
        public InventoryController(MakerTrackerContext context) : base(context)
        {
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTransactions()
        {
            var profile = await GetLoggedInProfile();
            var transactions = _context.Transactions.Where(x => x.To == profile || x.From == profile)
                .Select(t => new
                {
                    ProductId = t.Product.Id,
                    t.Product.Name,
                    t.Product.ImageUrl,
                    Amount = t.To == profile && t.NeedId == null ? t.Amount : -t.Amount,
                    t.Id
                });

            var model = await transactions
                .GroupBy(x => new { x.ProductId, x.Name, x.ImageUrl })
                .Select(t => new InventoryProductSummaryDto
                {
                    ProductId = t.Key.ProductId,
                    ProductName = t.Key.Name,
                    ProductImageUrl = t.Key.ImageUrl,
                    Amount = t.Sum(x => x.Amount)
                }).Where(s => s.Amount > 0).ToListAsync();

            return model;
        }

        // GET: api/Inventory/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Inventory/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, InventoryTransactionDto model)
        {
            if (id != model.Product.Id)
            {
                return BadRequest("Product ID's don't match");
            }

            var profile = await GetLoggedInProfile();
            await SetProductInventoryValue(model, profile);

            return Ok(true);
        }

        protected async Task SetProductInventoryValue(InventoryTransactionDto model, Profile profile, bool autoCommit = true)
        {
            var product = await _context.Products.FindAsync(model.Product.Id);
            var currentAmount = await _context.Transactions.Where(x => x.To == profile || x.From == profile)
                .Where(x => x.Product == product)
                .Select(t => t.To == profile && t.NeedId == null ? t.Amount : -t.Amount)
                .SumAsync();

            //positive amount means they are increasing the amount
            var difference = model.Amount - currentAmount;

            if (difference != 0)
            {
                var transaction = new Transaction()
                {
                    Amount = difference,
                    From = null,
                    To = profile,
                    TransactionDate = DateTime.Now,
                    TransactionType = TransactionType.Stock,
                    Status = TransactionStatus.Confirmed,
                    Product = product,
                    ConfirmationDate = DateTime.Now,
                };

                _context.Transactions.Add(transaction);
                if (autoCommit)
                {
                    await _context.SaveChangesAsync();
                }
            }
        }

        // POST: api/Inventory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(InventoryTransactionDto model)
        {
            var profile = await GetLoggedInProfile();
            await CreateTransaction(model, profile);
            await _context.SaveChangesAsync();

            return Ok(true);
            //return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // POST: api/Inventory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("bulk")]
        public async Task<ActionResult<Transaction>> PostTransactions(InventoryTransactionDto[] entries)
        {
            var profile = await GetLoggedInProfile();
            foreach (var entry in entries)
            {
                await SetProductInventoryValue(entry, profile, false);
            }
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        private async Task<Transaction> CreateTransaction(InventoryTransactionDto entry, Profile profile)
        {
            var product = await _context.Products.FindAsync(entry.Product.Id);
            var to = entry.NeedId != null ? (await _context.Needs.FindAsync(entry.NeedId)).Profile : profile;
            var from = entry.NeedId != null ? profile : null;
            var transaction = new Transaction()
            {
                Amount = entry.Amount,
                From = from,
                To = to,
                TransactionDate = DateTime.Now,
                TransactionType = TransactionType.Stock,
                Status = TransactionStatus.Confirmed,
                Product = product,
                ConfirmationDate = DateTime.Now,
                NeedId = entry.NeedId
            };

            _context.Transactions.Add(transaction);
            return transaction;
        }

        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
