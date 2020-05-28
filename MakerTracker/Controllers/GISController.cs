using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using FizzWare.NBuilder;
using MakerTracker.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

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

        [HttpGet()]
        public IActionResult DownloadShapeFile()
        {
            return new PhysicalFileResult(GetZipFilePath(), "application/zip");
        }

        [HttpGet("generateShapeFile")]
        public async Task<IActionResult> GenerateFile()
        {
            var data = await _context.Profiles.Select(p => new
            {
                p.FirstName,
                p.LastName,
                p.Latitude,
                p.Longitude,
                p.CompanyName,
                p.IsDropOffPoint,
                p.Phone,
                p.IsRequestor,
                p.IsSupplier,

            }).ToListAsync();

            var geomFactory = NtsGeometryServices.Instance.CreateGeometryFactory();
            var features = new List<Feature>();

            var faker = new Bogus.Faker("en");
            foreach (var d in data)
            {
                //note that attribute names can be at most 11 characters
                var t1 = new AttributesTable
                {
                    {"firstName", d.FirstName ?? ""},
                    {"lastName", d.LastName ?? ""},
                    {"companyName", d.CompanyName ?? ""},
                    {"dropOff", d.IsDropOffPoint.GetValueOrDefault().ToString()},
                    {"type", DetermineType(d.IsRequestor,d.IsSupplier) },
                    {"phoneNumber", d.Phone ?? ""},
                    {"amount1", faker.Random.Number(100,10000) },
                    {"product1", faker.Commerce.ProductName() }
                };

                Geometry g1 = geomFactory.CreatePoint(new Coordinate(d.Longitude, d.Latitude));

                var feat1 = new Feature(g1, t1);
                features.Add(feat1);
            }

            var shapeFileFolder = GetShapeFilePath();
            var zipPath = GetZipFilePath();
            var shapefileName = Path.Combine(shapeFileFolder, "maker");

            var writer = new ShapefileDataWriter(shapefileName)
            { Header = ShapefileDataWriter.GetHeader(features[0], features.Count) };

            writer.Write(features);

            if (System.IO.File.Exists(zipPath))
            {
                System.IO.File.Delete(zipPath);
            }

            ZipFile.CreateFromDirectory(shapeFileFolder, zipPath, CompressionLevel.Fastest, false);

            await using (var csvWriter = new StreamWriter(GetCSVFilePath()))
            await using (var csv = new CsvWriter(csvWriter, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }


            return Ok($"Files created and zipped to {zipPath}");
        }

        private static string DetermineType(bool isRequestor, bool isSupplier)
        {
            var t = "None";
            if (isRequestor && isSupplier)
                t = "Both";
            else
            {
                if (isRequestor) t = "Requestor";
                if (isSupplier) t = "Supplier";
            }
            return t;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("generateTestData")]
        public async Task<IActionResult> GenerateTestData()
        {
            var faker = new Bogus.Faker("en");

            BuilderSetup.DisablePropertyNamingFor<Product, int>(x => x.Id);
            BuilderSetup.DisablePropertyNamingFor<Profile, int>(x => x.Id);
            BuilderSetup.DisablePropertyNamingFor<Transaction, int>(x => x.Id);
            BuilderSetup.DisablePropertyNamingFor<ProductType, int>(x => x.Id);

            int productTypeCount = 3;
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
                    .With(p => p.ImageUrl = $"https://i.picsum.photos/id/" + p.Id + "/400/400.jpg")
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
                    .With(p => p.Location = new Point(faker.Address.Longitude(-94.6198, -89.6594), faker.Address.Latitude(33.0075, 36.4997)) { SRID = 4326 }) // min&max of Arkansas
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

        private string GetZipFilePath()
        {
            return Path.Combine(_env.WebRootPath, "giszip", "makertracker.zip");
        }
        private string GetCSVFilePath()
        {
            return Path.Combine(_env.WebRootPath, "giszip", "makertracker.csv");
        }

        private string GetShapeFilePath()
        {
            return Path.Combine(_env.WebRootPath, "gis");
        }

    }
}