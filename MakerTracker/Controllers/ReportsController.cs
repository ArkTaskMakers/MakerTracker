using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using MakerTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace MakerTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly MakerTrackerContext _context;

        public ReportsController(MakerTrackerContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var makerTrackerContext = _context.MakerOrder.Select(mo => new ReportMakerWork
            {
                FirstName = mo.Maker.OwnerProfile.FirstName,
                LastName = mo.Maker.OwnerProfile.LastName,
                ProductId = mo.Product.Id,
                ProductName = mo.Product.Name,
                ExpectedFinished = mo.ExpectedFinished,
                IsFinished = mo.Finished,
                ExpectedCount = mo.PromisedCount,
            }).OrderBy(x=>x.ProductId).ThenByDescending(x=>x.IsFinished).ThenByDescending(x=>x.ExpectedFinished);

            return View(await makerTrackerContext.ToListAsync());
        }

        public async Task<IActionResult> Makers()
        {
            var makers = _context.Makers.Select(m => new ReportMaker()
            {
                FirstName = m.OwnerProfile.FirstName,
                LastName = m.OwnerProfile.LastName,
                City = m.OwnerProfile.City,
                State = m.OwnerProfile.State,
                ZipCode = m.OwnerProfile.ZipCode,
                ProductsFinished = m.MakerQueue.Where(mq=>mq.Finished).Sum(x=>x.PromisedCount),
                ProductsQueued = m.MakerQueue.Where(mq=>mq.Finished == false).Sum(x=>x.PromisedCount),

            }).OrderBy(x => x.LastName).ThenByDescending(x => x.FirstName);

            return View(await makers.ToListAsync());
        }

        
    }
}
