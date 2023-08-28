﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.Data.EF.Models;
using System.Drawing;
using StoreFront.UI.MVC.Utilities;

namespace StoreFront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(StoreFrontContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            //var storeFrontContext = _context.Products.Include(p => p.Category).Include(p => p.Merchant).Include(p => p.ProductStatus).Include(p => p.Season);
            //return View(await storeFrontContext.ToListAsync());

            var products = _context.Products.Where(p => p.ProductStatusId != 5)
                .Include(p => p.Category)
                .Include(p => p.Merchant)
                .Include(p => p.Season);

            return View(await products.ToListAsync());
        }


        // GET: Products/TiledProducts
        public async Task<IActionResult> TiledProducts()
        {
            var products = _context.Products.Where(p => p.ProductStatusId != 5)
                .Include(p => p.Category)
                .Include(p => p.Merchant)
                .Include(p => p.Season);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Merchant)
                .Include(p => p.ProductStatus)
                .Include(p => p.Season)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["MerchantId"] = new SelectList(_context.Merchants, "MerchantId", "MerchantName");
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusName");
            ViewData["SeasonId"] = new SelectList(_context.SeasonalAvailabilities, "SeasonId", "SeasonCategory");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,ProductStatusId,SeasonId,MerchantId,ProductImage,Image,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Image != null)
                {
                    string ext = Path.GetExtension(product.Image.FileName);
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        product.ProductImage = Guid.NewGuid() + ext;
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullImagePath = webRootPath + "/assets/img/products/";
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }
                else
                {

                    product.ProductImage = "noimage.png";
                }


                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["MerchantId"] = new SelectList(_context.Merchants, "MerchantId", "MerchantName", product.MerchantId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusName", product.ProductStatusId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonalAvailabilities, "SeasonId", "SeasonCategory", product.SeasonId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["MerchantId"] = new SelectList(_context.Merchants, "MerchantId", "MerchantName", product.MerchantId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusName", product.ProductStatusId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonalAvailabilities, "SeasonId", "SeasonCategory", product.SeasonId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductDescription,ProductStatusId,SeasonId,MerchantId,ProductImage,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["MerchantId"] = new SelectList(_context.Merchants, "MerchantId", "MerchantName", product.MerchantId);
            ViewData["ProductStatusId"] = new SelectList(_context.ProductStatuses, "ProductStatusId", "StatusName", product.ProductStatusId);
            ViewData["SeasonId"] = new SelectList(_context.SeasonalAvailabilities, "SeasonId", "SeasonCategory", product.SeasonId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Merchant)
                .Include(p => p.ProductStatus)
                .Include(p => p.Season)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'StoreFrontContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
