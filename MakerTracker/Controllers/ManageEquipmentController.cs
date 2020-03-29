using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;
using Microsoft.AspNetCore.Authorization;

namespace MakerTracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageEquipmentController : Controller
    {
        private readonly MakerTrackerContext _context;

        public ManageEquipmentController(MakerTrackerContext context)
        {
            _context = context;
        }

        // GET: ManageEquipment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Equipments.ToListAsync());
        }

        // GET: ManageEquipment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: ManageEquipment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ManageEquipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(equipment);
        }

        // GET: ManageEquipment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment == null)
            {
                return NotFound();
            }
            return View(equipment);
        }

        // POST: ManageEquipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquipmentExists(equipment.Id))
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
            return View(equipment);
        }

        // GET: ManageEquipment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: ManageEquipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EquipmentExists(int id)
        {
            return _context.Equipments.Any(e => e.Id == id);
        }
    }
}
