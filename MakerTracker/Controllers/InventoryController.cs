namespace MakerTracker.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using MakerTracker.DBModels;
    using MakerTracker.Models.Inventory;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    [Authorize()]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ApiBaseController
    {
        private SendGridClient SendGridClient { get; }
        private MailSettings MailSettings { get; }
        private ILogger<InventoryController> Logger { get; }

        public InventoryController(MakerTrackerContext context, SendGridClient sendGridClient, MailSettings mailSettings, ILogger<InventoryController> logger) : base(context)
        {
            SendGridClient = sendGridClient;
            MailSettings = mailSettings;
            Logger = logger;
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

            var target = model.NeedId == null ? profile : await _context.Needs.Where(e => e.Id == model.NeedId).Select(e => e.Profile).SingleAsync();

            //positive amount means they are increasing the amount
            var difference = model.Amount - currentAmount;

            if (difference + currentAmount < 0)
            {
                throw new InvalidOperationException($"Unable to transact to negative: {JsonConvert.SerializeObject(model)}");
            }

            if (difference != 0)
            {
                var transaction = new Transaction()
                {
                    Amount = difference,
                    From = model.NeedId == null ? null : profile,
                    To = target,
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
            var transaction = await CreateTransaction(model, profile);
            await _context.SaveChangesAsync();

            if (transaction.NeedId != null)
            {
                try
                {
                    await NotifyDeliverySender(transaction);
                }
                catch (Exception exc)
                {
                    Logger.LogError(exc, $"Error sending delivery details to supplier: {JsonConvert.SerializeObject(transaction)}");
                }
            }

            return Ok(true);
        }

        protected async Task NotifyDeliverySender(Transaction transaction)
        {
            var to = transaction.To;
            var displayName = string.IsNullOrWhiteSpace(to.CompanyName) ? $"{to.FirstName} {to.FirstName}" : to.CompanyName;
            var need = transaction.Need;
            var addressLine = HttpUtility.HtmlEncode(string.Join("<br />", new[] { to.Address, to.Address2 }.Where((a) => !string.IsNullOrWhiteSpace(a))));
            string address = $"<p><address>{addressLine}<br />{HttpUtility.HtmlEncode(to.City)}, ${HttpUtility.HtmlEncode(to.State)} ${to.ZipCode}</address></p>";
            string due = need.DueDate.HasValue
                ? $"<p>It needs to be delivered by {need.DueDate.GetValueOrDefault().ToShortDateString()} at the latest.</p>"
                : string.Empty;
            string contact = $"<p>If you have any questions, please contact <strong>{displayName}</strong> at <a href=\"mailto:{to.Email}\">{to.Email}</a></p>";
            string specialInstructions = string.IsNullOrWhiteSpace(need.SpecialInstructions) ? string.Empty
                : $"<p><strong>Special Instructions from requestor:</strong><br />${HttpUtility.HtmlEncode(need.SpecialInstructions)}</p>";

            // TODO: Rough draft of this. We should discuss
            string disclaimer = "<p><em>If you have received this email in error, please notify pgordon@arhub.org</em></p>";

            string body = $"<p>Thank you for delivering this PPE! This organization's name is {displayName}. Their address is as follows:</p>{address}{due}{contact}{specialInstructions}{disclaimer}";

            string plainTextBody = Regex.Replace(body.Replace("</p>", $"{Environment.NewLine}{Environment.NewLine}").Replace("<br />", Environment.NewLine).Replace("<p>", string.Empty), "<[^>]*(>|$)", string.Empty);

            SendGridMessage message = MailHelper.CreateSingleEmail(
                new EmailAddress("no-reply@arhub.org"),
                new EmailAddress(transaction.From.Email),
                $"PPE Delivery Details", plainTextBody, body);
            message.MailSettings = MailSettings;
            await SendGridClient.SendEmailAsync(message);
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

            var currentAmount = await _context.Transactions.Where(x => x.To == profile || x.From == profile)
                .Where(x => x.Product == product)
                .Select(t => t.To == profile && t.NeedId == null ? t.Amount : -t.Amount)
                .SumAsync();

            if (from != null && currentAmount < entry.Amount)
            {
                throw new InvalidOperationException($"Unable to transact to negative: {JsonConvert.SerializeObject(entry)}");
            }

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
