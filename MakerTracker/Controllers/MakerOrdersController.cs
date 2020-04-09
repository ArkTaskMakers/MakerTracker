using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using MakerTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace MakerTracker.Controllers
{
    [Authorize()]
    public class MakerOrdersController : BaseController
    {
        private readonly MakerTrackerContext _context;

        public MakerOrdersController(MakerTrackerContext context)
        {
            _context = context;
        }

        // GET: MakerOrders
        public async Task<IActionResult> Index()
        {
            var makerOrders = await _context.MakerOrders
                .Include(x => x.Product)
                .Where(p => p.Maker.OwnerProfile.Auth0Id == User.Identity.Name).ToListAsync();
            return View(makerOrders);
        }

        // GET: MakerOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makerOrder = await _context.MakerOrders
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
        public async Task<IActionResult> Create(CreateEditMakerOrder makerOrder)
        {
            if (ModelState.IsValid)
            {
                var maker = GetMakerProfile(User.Identity.Name);
                var newOrder = new MakerOrder()
                {
                    ExpectedFinished = makerOrder.ExpectedFinished,
                    OrderedOn = DateTime.Now,
                    ProductId = makerOrder.ProductId,
                    PromisedCount = makerOrder.PromisedCount,
                    Finished = makerOrder.Finished,
                    Maker = maker
                };
                maker.MakerQueue.Add(newOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.AvailableProducts = _context.Products.ToList();
            return View(makerOrder);
        }

        //TODO move to a service
        private Maker GetMakerProfile(string auth0Id)
        {
            var profile = _context.Profiles.FirstOrDefault(p => p.Auth0Id == auth0Id);

            if (profile == null)
            {
                var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var firstName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
                var lastName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;

                profile = new Profile()
                {
                    Email = email,
                    CreatedDate = DateTime.Now,
                    FirstName = firstName,
                    LastName = lastName,
                    Auth0Id = auth0Id
                };
                _context.Profiles.Add(profile);
                _context.SaveChanges();
            }

            var maker = _context.Makers.FirstOrDefault(m => m.OwnerProfile == profile);
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

            var makerOrder = await _context.MakerOrders.FirstOrDefaultAsync(x=>x.Id == id && x.Maker.OwnerProfile.Auth0Id == User.Identity.Name);
            if (makerOrder == null)
            {
                return NotFound();
            }

            ViewBag.AvailableProducts = _context.Products.ToList();

            var model = new CreateEditMakerOrder()
            {
                MakerOrderId = makerOrder.Id,
                ExpectedFinished = makerOrder.ExpectedFinished,
                ProductId = makerOrder.ProductId,
                PromisedCount = makerOrder.PromisedCount,
                Finished = makerOrder.Finished,
            };
            return View(model);
        }

        // POST: MakerOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateEditMakerOrder makerOrder)
        {
            var existingOrder = await _context.MakerOrders.FirstOrDefaultAsync(x=>x.Id == id && x.Maker.OwnerProfile.Auth0Id == User.Identity.Name);
            if (existingOrder == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               existingOrder.ProductId = makerOrder.ProductId;
               existingOrder.PromisedCount = makerOrder.PromisedCount;
               existingOrder.ExpectedFinished = makerOrder.ExpectedFinished;
               existingOrder.Finished = makerOrder.Finished;

                try
                {
                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MakerOrderExists(id))
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

            ViewBag.AvailableProducts = _context.Products.ToList();
            return View(makerOrder);
        }

        // GET: MakerOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var makerOrder = await _context.MakerOrders
                .FirstOrDefaultAsync(x=>x.Id == id && x.Maker.OwnerProfile.Auth0Id == User.Identity.Name);
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
            var makerOrder = await _context.MakerOrders.FirstOrDefaultAsync(x=>x.Id == id && x.Maker.OwnerProfile.Auth0Id == User.Identity.Name);
            _context.MakerOrders.Remove(makerOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MakerOrderExists(int id)
        {
            return _context.MakerOrders.Any(e => e.Id == id);
        }
    }
}
