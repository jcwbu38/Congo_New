using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test5.Models;
using Microsoft.EntityFrameworkCore;


namespace test5.Controllers
{
    public class HomeController : Controller
    {
        private readonly InventoryContext _context;
        public static List<Models.ShoppingCart.ShoppingCart> shoppingCart = new List<Models.ShoppingCart.ShoppingCart>();

        public HomeController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from m in _context.Inventory
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.description.Contains(searchString));
            }

            return View(await products.ToListAsync());

            //return View(await _context.Inventory.ToListAsync());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> ProductDetails( int? id )
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .SingleOrDefaultAsync(m => m.itemID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        public async Task<IActionResult> Add(int? id, int qty)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .SingleOrDefaultAsync(m => m.itemID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            var newItemToCart = new Models.ShoppingCart.ShoppingCart() {
                id = inventory.itemID,
                price = inventory.price,
                description = inventory.description,
                discountPrice = inventory.discountPrice,
                image = inventory.image,
                inventoryQuantity = inventory.quantity,
                cartQuantity = qty 
            };

            shoppingCart.Add(newItemToCart);
            var item = await _context.Inventory
                .SingleOrDefaultAsync(m => m.itemID == id);
            if (item == null)
            {
                return NotFound();
            }

            // Would like to reload the Product Details page instead of going to shopping cart
            // Update product quantity

            //return RedirectToAction("Index", "ProductDetails", newItemToCart.id);
            return RedirectToAction("Index", "ShoppingCart", newItemToCart);

        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
