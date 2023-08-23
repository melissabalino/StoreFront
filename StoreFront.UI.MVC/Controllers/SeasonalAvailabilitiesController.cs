using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.Data.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    public class SeasonalAvailabilitiesController : Controller
    {
        private readonly StoreFrontContext _context;

        public SeasonalAvailabilitiesController(StoreFrontContext context)
        {
            _context = context;
        }

        // GET: SeasonalAvailabilities
        public async Task<IActionResult> Index()
        {
              return _context.SeasonalAvailabilities != null ? 
                          View(await _context.SeasonalAvailabilities.ToListAsync()) :
                          Problem("Entity set 'StoreFrontContext.SeasonalAvailabilities'  is null.");
        }

        // GET: SeasonalAvailabilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SeasonalAvailabilities == null)
            {
                return NotFound();
            }

            var seasonalAvailability = await _context.SeasonalAvailabilities
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (seasonalAvailability == null)
            {
                return NotFound();
            }

            return View(seasonalAvailability);
        }

        // GET: SeasonalAvailabilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SeasonalAvailabilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeasonId,SeasonCategory,SeasonDescription")] SeasonalAvailability seasonalAvailability)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seasonalAvailability);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(seasonalAvailability);
        }

        // GET: SeasonalAvailabilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SeasonalAvailabilities == null)
            {
                return NotFound();
            }

            var seasonalAvailability = await _context.SeasonalAvailabilities.FindAsync(id);
            if (seasonalAvailability == null)
            {
                return NotFound();
            }
            return View(seasonalAvailability);
        }

        // POST: SeasonalAvailabilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeasonId,SeasonCategory,SeasonDescription")] SeasonalAvailability seasonalAvailability)
        {
            if (id != seasonalAvailability.SeasonId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seasonalAvailability);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonalAvailabilityExists(seasonalAvailability.SeasonId))
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
            return View(seasonalAvailability);
        }

        // GET: SeasonalAvailabilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SeasonalAvailabilities == null)
            {
                return NotFound();
            }

            var seasonalAvailability = await _context.SeasonalAvailabilities
                .FirstOrDefaultAsync(m => m.SeasonId == id);
            if (seasonalAvailability == null)
            {
                return NotFound();
            }

            return View(seasonalAvailability);
        }

        // POST: SeasonalAvailabilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SeasonalAvailabilities == null)
            {
                return Problem("Entity set 'StoreFrontContext.SeasonalAvailabilities'  is null.");
            }
            var seasonalAvailability = await _context.SeasonalAvailabilities.FindAsync(id);
            if (seasonalAvailability != null)
            {
                _context.SeasonalAvailabilities.Remove(seasonalAvailability);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonalAvailabilityExists(int id)
        {
          return (_context.SeasonalAvailabilities?.Any(e => e.SeasonId == id)).GetValueOrDefault();
        }
    }
}
