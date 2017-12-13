/* ClearanceController.cs
 * This contoller provides access to items that are marked as clearance.
 * This file covers requirement 3.2.10.3.1.

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test5.Models;
using Microsoft.EntityFrameworkCore;

namespace test5.Controllers
{
    [AllowAnonymous]
    public class ClearanceController : Controller
    {
        private readonly InventoryContext _context;

        public ClearanceController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory - Retrieves data for the clearance webpage view and sends it to Index.cshtml for formatting. Also handles search queries.
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from m in _context.Inventory
                           select m;

            if (!String.IsNullOrEmpty(searchString)) // section 4.17, requirement 3.2.15.3.1
            {
                products = products.Where(s => s.description.ToUpper().Contains(searchString.ToUpper()));
            }
            return View(await products.ToListAsync());
            //return View(await _context.Inventory.ToListAsync());
        }
        
    }
}