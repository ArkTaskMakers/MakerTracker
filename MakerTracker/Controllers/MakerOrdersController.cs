using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using MakerTracker.Models;

namespace MakerTracker.Controllers
{
    public class MakerOrdersController : Controller
    {
        private readonly MakerTrackerContext _context;

        public MakerOrdersController(MakerTrackerContext context)
        {
            _context = context;
        }

        // GET: MakerOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.MakerOrder.Include(x=>x.Product).ToListAsync());
        }

        // GET: MakerOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makerOrder = await _context.MakerOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (makerOrder == null)
            {
                return NotFound();
            }

            return View(makerOrder);
        }

        // GET: MakerOrders/Create
        public IActionResult Create()
        {
            ViewBag.AvailableProducts = _context.Products.ToList();
            return View();
        }

        // POST: MakerOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMakerOrder makerOrder)
        {
            if (ModelState.IsValid)
            {
                var maker = GetMakerProfile();
                var newOrder = new MakerOrder()
                {
                    ExpectedFinished = makerOrder.ExpectedFinished,
                    OrderedOn = DateTime.Now,
                    ProductId = makerOrder.ProductId,
                    PromisedCount = makerOrder.PromisedCount,
                    Maker = maker
                };
                maker.MakerQueue.Add(newOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(makerOrder);
        }

        //TODO move to a service
        private Maker GetMakerProfile()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profile = _context.Profiles.FirstOrDefault(p => p.Email == email);
            if (profile == null)
            {
                profile = new Profile()
                {
                    Email = email,
                    CreatedDate = DateTime.Now,
                    FirstName = User.Identity.Name,
                };
                _context.Profiles.Add(profile);
                _context.SaveChanges();
            }

            var maker = _context.Makers.FirstOrDefault(m=> m.OwnerProfile == profile);
            if (maker == null)
            {
                maker = new Maker()
                {
                    OwnerProfile = profile,
                    
                };
                _context.Makers.Add(maker);
                _context.SaveChanges();
            }

            return maker;

        }

        // GET: MakerOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makerOrder = await _context.MakerOrder.FindAsync(id);
            if (makerOrder == null)
            {
                return NotFound();
            }
            return View(makerOrder);
        }

        // POST: MakerOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderedOn,ExpectedFinished,PromisedCount")] MakerOrder makerOrder)
        {
            if (id != makerOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(makerOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakerOrderExists(makerOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(makerOrder);
        }

        // GET: MakerOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makerOrder = await _context.MakerOrder
                .FirstOrDefaultAsync(m => m.Id == id);
            if (makerOrder == null)
            {
                return NotFound();
            }

            return View(makerOrder);
        }

        // POST: MakerOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var makerOrder = await _context.MakerOrder.FindAsync(id);
            _context.MakerOrder.Remove(makerOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MakerOrderExists(int id)
        {
            return _context.MakerOrder.Any(e => e.Id == id);
        }
    }
}
