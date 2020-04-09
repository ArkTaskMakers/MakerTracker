using System.Collections.Generic;
using System.Linq;
using MakerTracker.DBModels;
using MakerTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MakerTracker.Controllers
{
    [Authorize()]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly MakerTrackerContext _db;
        public DashboardController(ILogger<DashboardController> logger, MakerTrackerContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var auth0Id = User.Identity.Name;
            var profile = _db.Profiles.FirstOrDefault(x => x.Auth0Id == auth0Id);
            var model = new DashboardViewModel()
            {
                WhatIHave = GetWhatIHave(profile)
            };

            return View(model);
        }

        public List<WhatIHaveModel> GetWhatIHave(Profile profile)
        {
            var transactions = _db.Transactions.Where(x => x.To == profile || x.From == profile)
                .Select(t => new
                {
                    ProductId = t.Product.Id,
                    t.Product.Name,
                    t.Product.ImageUrl,
                    Amount = t.To == profile ? t.Amount : -1 * t.Amount,
                    t.Id
                });

            var model = transactions
                .GroupBy(x => new { x.ProductId, x.Name, x.ImageUrl })
                .Select(t => new WhatIHaveModel
                {
                    ProductId = t.Key.ProductId,
                    ProductName = t.Key.Name,
                    ProductImageUrl = t.Key.ImageUrl,
                    Amount = t.Sum(x => x.Amount)
                }).ToList();

            return model;
        }


        public IActionResult WhatImMaking()
        {

            return PartialView();
        }
    }
}