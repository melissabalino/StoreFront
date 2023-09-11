using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.Data.EF.Models;
using Microsoft.AspNetCore.Authorization; 
using System.Drawing;
using StoreFront.UI.MVC.Utilities;

using X.PagedList;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(string searchTerm, int categoryId = 0, int merchantId = 0, int page = 1)
        {
          
            var products = _context.Products.Where(p => p.ProductStatusId != 5 && p.SeasonId == 1 || p.SeasonId == 4)
                .Include(p => p.Category)
                .Include(p => p.Merchant)
                .Include(p => p.Season)
                .Include(p => p.OrderDetails).ToList();

            #region search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                    p.ProductName.ToLower().Contains(searchTerm.ToLower())
                    || p.Merchant.MerchantName.ToLower().Contains(searchTerm.ToLower())
                    || p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())
                    || p.ProductDescription.ToLower().Contains(searchTerm.ToLower())).ToList();
                ViewBag.NbrResults = products.Count;
                ViewBag.SearchTerm = searchTerm;

            }
            #endregion

            #region Category Filter
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Category = 0;

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
                ViewBag.Category = categoryId;
            }
            #endregion

            #region Merchant Filter
            ViewBag.MerchantId = new SelectList(_context.Merchants, "MerchantId", "MerchantName", merchantId);
            ViewBag.Merchants = await _context.Merchants.ToListAsync();
            ViewBag.Merchant = 0;

            if (merchantId != 0)
            {
                products = products.Where(p => p.MerchantId == merchantId).ToList();
                ViewBag.Merchant = merchantId;
            }

            #endregion

            return View(products.ToPagedList(page, 24));
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> LawnAndGarden()
        //{
        //    var products = _context.Products.Where(p => p.ProductStatusId != 5 && p.CategoryId == 1)
        //        .Include(p => p.Category)
        //        .Include(p => p.Merchant)
        //        .Include(p => p.Season);
        //    return View(await products.ToListAsync());
        //}

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id, string? prevAction)
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

            ViewBag.PrevAction = null;
            if (prevAction == "Index")
            {
                ViewBag.PrevAction = prevAction;
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
                #region File Upload - CREATE
                //Check if a file was uploaded
                if (product.Image != null)
                {
                    //Check the file type of the image by retrieving the extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Create a list of valid extensions to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //Verify that the uploaded file has one of the appropriate extensions listed above
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unique file name for the image
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Save the file to the web server
                        //Retrieve the path to the wwwroot
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //Create the path for where we want to save the images
                        string fullImagePath = webRootPath + "/assets/img/products/";

                        //Create a MemoryStream to read the image into server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            //Transfer the file from the request into server memory
                            await product.Image.CopyToAsync(memoryStream);

                            //Create a copy of the image so we can manipulate and save it as needed
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 100;
                                int maxThumbSize = 48;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }
                else
                {
                    product.ProductImage = "noimage.png";
                }
                #endregion

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
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductDescription,ProductStatusId,SeasonId,MerchantId,ProductImage,Image,CategoryId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                #region File Upload - EDIT
                //Retain the old image file name so we can delete it if a new file was uploaded
                string oldImageName = product.ProductImage;
                
                //Check to see if the user uploaded a file
                if (product.Image != null)
                {
                    //Get the file extension
                    string ext = Path.GetExtension(product.Image.FileName);
                    
                    //Create a list of valid exts
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };
                    
                    //Check to ensure the extension is good and the image isn't too big
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unique file name
                        product.ProductImage = Guid.NewGuid() + ext;
                        
                        //Build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/assets/img/products/";

                        //Delete the old image
                        if (oldImageName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 100;
                                int maxThumbSize = 48;

                                ImageUtility.ResizeImage(fullPath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }
                #endregion

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
