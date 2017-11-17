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

        public HomeController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventory.ToListAsync());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> ProductDetails( int? id )
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        public async Task<ActionResult> Add(int? id, int qty)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }

            var newCart = new Models.ShoppingCart.Cart() { price = inventory.price, description = inventory.description, discountPrice = inventory.discountPrice, image = inventory.image, quantity = qty };
            return RedirectToAction("Index", "ShoppingCart", newCart);

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
