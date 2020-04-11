using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using MakerTracker.Models;
using MakerTracker.Models.Inventory;
using Microsoft.AspNetCore.Authorization;

namespace MakerTracker.Controllers
{
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
            var profile = this.GetLoggedInProfile();
            var transactions = _context.Transactions.Where(x => x.To == profile || x.From == profile)
                .Select(t => new
                {
                    ProductId = t.Product.Id,
                    t.Product.Name,
                    t.Product.ImageUrl,
                    Amount = t.To == profile ? t.Amount : -1 * t.Amount,
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
                }).ToListAsync();

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
        public async Task<IActionResult> PutTransaction(int id, EditInventoryDto model)
        {
            if (id != model.ProductId)
            {
                return BadRequest("Product ID's don't match");
            }

            var profile = this.GetLoggedInProfile();
            var product = _context.Products.Find(model.ProductId);
            var currentAmount = _context.Transactions.Where(x => x.To == profile || x.From == profile)
                .Where(x => x.Product == product)
                .Select(t => new
                {
                    Amount = t.To == profile ? t.Amount : -1 * t.Amount,
                })
                .Sum(x => x.Amount);

            //positive amount means they are increasing the amount
            var difference = model.NewAmount - currentAmount;

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
                await _context.SaveChangesAsync();
            }

            return Ok(true);
        }

        // POST: api/Inventory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(AddInventoryDto model)
        {
            var profile = this.GetLoggedInProfile();
            var product = _context.Products.Find(model.ProductId);
            var transaction = new Transaction()
            {
                Amount = model.Amount,
                From = null,
                To = profile,
                TransactionDate = DateTime.Now,
                TransactionType = TransactionType.Stock,
                Status = TransactionStatus.Confirmed,
                Product = product,
                ConfirmationDate = DateTime.Now,
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(true);
            //return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
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
