using Microsoft.AspNetCore.Mvc;

using StoreFront.Data.EF.Models;
using Microsoft.AspNetCore.Identity;
using StoreFront.UI.MVC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace StoreFront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly StoreFrontContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartController(StoreFrontContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            string? sessionCart = HttpContext.Session.GetString("cart");
            Dictionary<int, CartItemViewModel> shoppingCart;
            if (string.IsNullOrEmpty(sessionCart))
            {
                shoppingCart = new();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart) ?? new();
            }

            if (!shoppingCart.Any())
            {
                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;
                ViewBag.Total = shoppingCart.Values.Sum(x => x.Product.ProductPrice * x.Qty).ToString("c");
            }
            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            Dictionary<int, CartItemViewModel> shoppingCart;
            string? sessionCart = HttpContext.Session.GetString("cart");
            if (string.IsNullOrEmpty(sessionCart)) 
            {
                shoppingCart = new();
            }
            else
            {
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart) ?? new();
            }

            Product? product = _context.Products.Find(id);

            if (shoppingCart.ContainsKey(product.ProductId))
            {
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                shoppingCart.Add(product.ProductId, new(1, product));
            }

            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            if (id == null)
            {
                ViewBag.Message = "ERROR";
                return RedirectToAction("Index");
            }
            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            shoppingCart?.Remove(id);
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");

        }

        public IActionResult UpdateCart(int productId, int qty)
        {
            if (qty <= 0)
            {
                RemoveFromCart(productId);
            }
            else
            {
                var sessionCart = HttpContext.Session.GetString("cart");
                var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
                shoppingCart[productId].Qty = qty;
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }
            return RedirectToAction("Index");

        }
        [Authorize]
        public async Task<IActionResult> CheckoutAsync()
        {
            var sessionCart = HttpContext.Session.GetString("cart");
            var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            ViewBag.Total = shoppingCart.Sum(x => x.Value.Qty * x.Value.Product.ProductPrice).ToString("c");
            ViewBag.UserId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SubmitOrder([Bind("OrderId,UserId,OrderDate,ShipToName,ShipToCity,ShipToState,ShipToZip")] Order order)
        {
            if (ModelState.IsValid)
            {
                var sessionCart = HttpContext.Session.GetString("cart");
                var shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
                foreach (var item in shoppingCart.Values)
                {
                    order.OrderDetails.Add(new()
                    {
                        OrderId = order.OrderId,
                        ProductId = item.Product.ProductId,
                       // ProductPrice = item.Product.ProductPrice,
                        Quantity = item.Qty
                    });
                }

                _context.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("cart");
                return RedirectToAction("Index", "Orders");
            }
            return View("Checkout", order);
        }

    }
}
