/* NewProductController.cs
 * This contoller provides access to items that are marked as new.
 * This file covers requirement 3.2.10.3.1.

*/
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test5.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace test5.Controllers
{
    public class NewProductController : Controller
    {
        private readonly InventoryContext _context;

        public NewProductController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory - Retrieves data for the New webpage view and sends it to Index.cshtml for formatting. Also handles search queries.
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from m in _context.Inventory
                           select m;

            if (!String.IsNullOrEmpty(searchString)) // section 4.17. requirement 3.2.15.3.1
            {
                products = products.Where(s => s.description.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(await products.ToListAsync());
        }
    }
}