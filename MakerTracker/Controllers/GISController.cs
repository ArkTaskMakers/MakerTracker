using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using CsvHelper;
using FizzWare.NBuilder;
using MakerTracker.DBModels;
using MakerTracker.Models.Inventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace MakerTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GISController : ApiBaseController
    {
        private readonly IWebHostEnvironment _env;


        public GISController(IWebHostEnvironment env, MakerTrackerContext context) : base(context)
        {
            _env = env;
        }


        [HttpGet("csv")]
        public IActionResult DownloadCSVFile()
        {
            return new PhysicalFileResult(GetCSVFilePath(), "application/csv");
        }


        [HttpGet("generatecsvfile")]
        public async Task<IActionResult> GenerateFile()
        {
            var data = await _context.Profiles.Select(p => new
            {
                ProfileId = p.Id,
                p.FirstName,
                p.LastName,
                p.Latitude,
                p.Longitude,
                p.CompanyName,
                p.IsDropOffPoint,
                p.Phone,
                p.IsRequestor,
                p.IsSupplier,
                p.Address,
                p.Address2,
                p.City,
                p.State,
                p.ZipCode,
                Needs = p.Needs.Where(n => n.FulfilledDate == null).Select(n => new
                {
                    n.DueDate,
                    n.Product.Name,
                    n.Quantity
                })
            }).ToListAsync();

            var transactions = _context.Transactions
                .Select(t => new
                {
                    To = t.To.Id,
                    From = t.From.Id,
                    ProductId = t.Product.Id,
                    ProductName = t.Product.Name,
                    t.Amount,
                    t.NeedId
                }).ToList();

            var output = new List<Dictionary<string, string>>();
            foreach (var d in data)
            {
                var row = new Dictionary<string, string>()
                {
                    {"Name", (d.FirstName ?? "") + " " + (d.LastName ?? "")},
                    {"Organization", d.CompanyName ?? ""},
                    {"Latitude", d.Latitude.ToString(CultureInfo.InvariantCulture)},
                    {"Longitude", d.Longitude.ToString(CultureInfo.InvariantCulture)},
                    {"Supplier or Requestor", DetermineType(d.IsRequestor, d.IsSupplier)},
                    {"Is Drop-off Point", d.IsDropOffPoint.GetValueOrDefault() ? "Yes"  : "No"},
                    {"Phone", d.IsRequestor ? d.Phone ?? "" : ""}
                };

                if (d.IsDropOffPoint.GetValueOrDefault())
                {
                    row.Add("Drop-off Address", d.Address ?? "" + " " + d.Address2);
                    row.Add("Drop-off City", d.City ?? "");
                    row.Add("Drop-off State", d.State ?? "");
                    row.Add("Drop-off Zip", d.ZipCode ?? "");
                }
                else
                {
                    row.Add("Drop-off Address", "");
                    row.Add("Drop-off City", "");
                    row.Add("Drop-off State", "");
                    row.Add("Drop-off Zip", "");
                }

                foreach (var need in d.Needs.Select((value, i) => (value, i)))
                {
                    row.Add($"Need" + (need.i + 1), $"{need.value.Name}: {need.value.Quantity} ");
                }

                var matchingTransactions = transactions.Where(x => x.To == d.ProfileId || x.From == d.ProfileId)
                    .Select(t => new
                    {
                        t.ProductId,
                        t.ProductName,
                        Amount = t.To == d.ProfileId && t.NeedId == null ? t.Amount : -t.Amount
                    });

                var inventory = matchingTransactions
                    .GroupBy(x => new { x.ProductId, x.ProductName })
                    .Select(t => new InventoryProductSummaryDto
                    {
                        ProductId = t.Key.ProductId,
                        ProductName = t.Key.ProductName,
                        Amount = t.Sum(x => x.Amount)
                    }).Where(s => s.Amount > 0);

                foreach (var inv in inventory.Select((value, i) => (value, i)))
                {
                    row.Add("Inventory " + (inv.i + 1), $"{inv.value.ProductName}: {inv.value.Amount}");
                }

                output.Add(row);

            }

            var csvPath = GetCSVFilePath();
            await using (var csvWriter = new StreamWriter(GetCSVFilePath()))
            await using (var csv = new CsvWriter(csvWriter, CultureInfo.InvariantCulture))
            {
                csv.WriteField("Name");
                csv.WriteField("Organization");
                csv.WriteField("Latitude");
                csv.WriteField("Longitude");
                csv.WriteField("Supplier or Requestor");
                csv.WriteField("Is Drop-off Point");
                csv.WriteField("Phone");
                csv.WriteField("Drop-off Address");
                csv.WriteField("Drop-off City");
                csv.WriteField("Drop-off State");
                csv.WriteField("Drop-off Zip");

                await csv.NextRecordAsync();
                foreach (var row in output)
                {
                    foreach (var col in row)
                    {
                        csv.WriteField(col.Value);
                    }
                    await csv.NextRecordAsync();
                }
            }
            return Ok($"CSV file create to {csvPath}");
        }

        private static string DetermineType(bool isRequestor, bool isSupplier)
        {
            return isRequestor && isSupplier ? "Both"
                : isRequestor ? "Requestor"
                : isSupplier ? "Supplier"
                : "None";
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("generateTestData")]
        public async Task<IActionResult> GenerateTestData()
        {
            var faker = new Faker();

            BuilderSetup.DisablePropertyNamingFor<Product, int>(x => x.Id);
            BuilderSetup.DisablePropertyNamingFor<Profile, int>(x => x.Id);
            BuilderSetup.DisablePropertyNamingFor<Transaction, int>(x => x.Id);
            BuilderSetup.DisablePropertyNamingFor<ProductType, int>(x => x.Id);

            var productTypeCount = 3;
            var productTypes = Builder<ProductType>.CreateListOfSize(productTypeCount)
                .All()
                .With(p => p.Name = faker.Commerce.Categories(1)[0])
                .With(p => p.SortOrder = faker.Random.Number(0, productTypeCount))
                .Build().ToList();

            _context.ProductTypes.AddRange(productTypes);
            await _context.SaveChangesAsync();

            var products = Builder<Product>.CreateListOfSize(20)
                .All()
                .With(p => p.ProductTypeId = faker.PickRandom(productTypes.Select(x => x.Id)))
                .With(p => p.Name = faker.Commerce.ProductName())
                .With(p => p.Description = faker.Commerce.ProductAdjective())
                .With(p => p.ImageUrl = "https://i.picsum.photos/id/" + p.Id + "/400/400.jpg")
                .Build().ToList();

            _context.Products.AddRange(products);
            await _context.SaveChangesAsync();


            var profiles = Builder<Profile>.CreateListOfSize(300)
                .All()
                .With(p => p.FirstName = faker.Name.FirstName())
                .With(p => p.LastName = faker.Name.LastName())
                .With(p => p.Address = faker.Address.StreetAddress())
                .With(p => p.City = faker.Address.City())
                .With(p => p.State = faker.Address.State())
                .With(p => p.ZipCode = faker.Address.ZipCode())
                .With(p => p.Bio = faker.Name.JobDescriptor())
                .With(p => p.Email = faker.Internet.Email())
                .With(p => p.IsDropOffPoint = false)
                .With(p => p.IsSelfQuarantined = false)
                .With(p => p.Phone = faker.Phone.PhoneNumber())
                .With(p => p.Latitude = faker.Address.Latitude(33.0075, 36.4997)) // min&max of Arkansas
                .With(p => p.Longitude = faker.Address.Longitude(-94.6198, -89.6594)) // min&max of Arkansas
                .With(p => p.Location = new Point(faker.Address.Longitude(-94.6198, -89.6594),
                    faker.Address.Latitude(33.0075, 36.4997))
                { SRID = 4326 }) // min&max of Arkansas
                .Random(10)
                .With(p => p.CompanyName == faker.Company.CompanyName())
                .Random(30)
                .With(p => p.CompanyName == faker.Company.CompanyName())
                .With(p => p.IsDropOffPoint == true)
                .Random(5)
                .With(p => p.IsSelfQuarantined == true)
                .Build();

            _context.Profiles.AddRange(profiles);
            await _context.SaveChangesAsync();


            var transactions = Builder<Transaction>.CreateListOfSize(1000)
                .All()
                .With(t => t.Product = faker.PickRandom(products))
                .With(t => t.Amount = faker.Random.Number(3, 10000))
                .With(t => t.Status = faker.PickRandom<TransactionStatus>())
                .With(t => t.TransactionType = TransactionType.Stock)
                .With(t => t.From = null)
                .With(t => t.To = faker.PickRandom(profiles))
                .With(t => t.TransactionDate = faker.Date.Between(new DateTime(2020, 04, 01), DateTime.Now))
                .With(t => t.NeedId = null)
                .Build();

            var transactions2 = Builder<Transaction>.CreateListOfSize(200)
                .All()
                .With(t => t.Product = faker.PickRandom(products))
                .With(t => t.Amount = faker.Random.Number(3, 10000))
                .With(t => t.Status = faker.PickRandom<TransactionStatus>())
                .With(t => t.TransactionType = TransactionType.Delivery)
                .With(t => t.From = faker.PickRandom(profiles))
                .With(t => t.To = faker.PickRandom(profiles))
                .With(t => t.TransactionDate = faker.Date.Between(new DateTime(2020, 04, 01), DateTime.Now))
                .With(t => t.NeedId = null)
                .Build();

            _context.Transactions.AddRange(transactions);
            _context.Transactions.AddRange(transactions2);
            await _context.SaveChangesAsync();

            return Ok("Populated");
        }

        private string GetCSVFilePath()
        {
            return Path.Combine(_env.WebRootPath, "giszip", "makertracker.csv");
        }
    }
}