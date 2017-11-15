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
    public class ClearanceController : Controller
    {
        private readonly InventoryContext _context;

        public ClearanceController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventory.ToListAsync());
        }
        
    }
}