/* HomeController.cs
 * This contoller provides access to the portal's main page as well as the product details pages and adding items to the shopping cart. 
 * This file covers design requirements 4.1 - 4.7, 4.14, and 4.17.

*/
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
        public static List<ShoppingCart> shoppingCart = new List<ShoppingCart>();

        public HomeController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory - Retrieves data for the main webpage view and sends it to Index.cshtml for formatting. Also handles search queries.
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from m in _context.Inventory select m;

            if (!String.IsNullOrEmpty(searchString)) // section 4.17. requirement 3.2.15.3.1
            {
                products = products.Where(s => s.description.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await products.ToListAsync());
        }

        // GET: Inventory/Details/ - Retrieves data for the specified item by ID and sends it to ProductDetails.cshtml for formatting.
        public async Task<IActionResult> ProductDetails( int? id )
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.SingleOrDefaultAsync(m => m.itemID == id);

            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // Add: ShoppingCart/Index/Item - Adds an item to the shopping cart based on the item ID and the quantity specified (Section 4.17).
        public async Task<IActionResult> Add(int? id, int qty)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.SingleOrDefaultAsync(m => m.itemID == id);

            if (inventory == null)
            {
                return NotFound();
            }

            var newItemToCart = new ShoppingCart() {
                id = inventory.itemID,
                price = inventory.price,
                description = inventory.description,
                discountPrice = inventory.discountPrice,
                image = inventory.image,
                inventoryQuantity = inventory.quantity,
                cartQuantity = qty, 
                stowLocation = inventory.locationID,
                productName = inventory.itemName
            };

            shoppingCart.Add(newItemToCart);

            var item = await _context.Inventory.SingleOrDefaultAsync(m => m.itemID == id);

            if (item == null)
            {
                return NotFound();
            }

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
