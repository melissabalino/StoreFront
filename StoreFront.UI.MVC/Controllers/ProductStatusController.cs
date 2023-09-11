using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.Data.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductStatusController : Controller
    {
        private readonly StoreFrontContext _context;

        public ProductStatusController(StoreFrontContext context)
        {
            _context = context;
        }

        // GET: ProductStatus
        public async Task<IActionResult> Index()
        {
            ViewBag.Products = _context.Products.Select(x => x.ProductStatusId).ToList();
            return _context.ProductStatuses != null ?
                        View(await _context.ProductStatuses.ToListAsync()) :
                        Problem("Entity set 'StoreFrontContext.ProductStatuses'  is null.");
        }

        // GET: ProductStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses
                .FirstOrDefaultAsync(m => m.ProductStatusId == id);
            if (productStatus == null)
            {
                return NotFound();
            }

            return View(productStatus);
        }

        // GET: ProductStatus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductStatusId,StatusName")] ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productStatus);
        }

        // GET: ProductStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductStatuses == null)
            {
                return NotFound();
            }

            var productStatus = await _context.ProductStatuses.FindAsync(id);
            if (productStatus == null)
            {
                return NotFound();
            }
            return View(productStatus);
        }

        // POST: ProductStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductStatusId,StatusName")] ProductStatus productStatus)
        {
            if (id != productStatus.ProductStatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductStatusExists(productStatus.ProductStatusId))
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
            return View(productStatus);
        }

        [AcceptVerbs("POST")]//[AcceptVerbs] lets you set multiple
        public JsonResult AjaxDelete(int id)
        {
            ProductStatus productStatus = _context.ProductStatuses.Find(id);
            _context.ProductStatuses.Remove(productStatus);
            _context.SaveChanges();

            string message = $"Deleted the Status, {productStatus.StatusName}, from the database!";
            return Json(new { id, message });
        }

        //#region original ProductStatus/Delete (Get and Post)
        //// GET: ProductStatus/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.ProductStatuses == null)
        //    {
        //        return NotFound();
        //    }

        //    var productStatus = await _context.ProductStatuses
        //        .FirstOrDefaultAsync(m => m.ProductStatusId == id);
        //    if (productStatus == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productStatus);
        //}

        //// POST: ProductStatus/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.ProductStatuses == null)
        //    {
        //        return Problem("Entity set 'StoreFrontContext.ProductStatuses'  is null.");
        //    }
        //    var productStatus = await _context.ProductStatuses.FindAsync(id);
        //    if (productStatus != null)
        //    {
        //        _context.ProductStatuses.Remove(productStatus);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //#endregion

        private bool ProductStatusExists(int id)
        {
          return (_context.ProductStatuses?.Any(e => e.ProductStatusId == id)).GetValueOrDefault();
        }
    }
}
