/* InventoryController.cs
 * This contoller provides access to the the inventory database for updating the contents.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test5.Models;
using Microsoft.AspNetCore.Authorization;

namespace test5.Controllers
{
    public class InventoryController : Controller
    {
        private readonly InventoryContext _context;

        public InventoryController(InventoryContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inventory.ToListAsync());
        }

        // GET: Inventory/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Inventory/Create
        [Authorize(Roles = "Sales, Tests")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Sales, Tests")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,itemID,itemName,dateReceived,locationID,sellerName,image,description,detailedDescription,price,discountPrice, quantity, EntryDate")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventory);
        }

        // GET: Inventory/Edit/5
        [Authorize(Roles = "Sales, Logistics, Tests")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.SingleOrDefaultAsync(m => m.ID == id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Sales, Logistics, Tests")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,itemID,itemName,dateReceived,locationID,sellerName,image,description,detailedDescription,price,discountPrice,quantity, entryDate")] Inventory inventory)
        {
            if (id != inventory.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.ID))
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
            return View(inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Sales, Tests")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventory = await _context.Inventory.SingleOrDefaultAsync(m => m.ID == id);
            _context.Inventory.Remove(inventory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventory.Any(e => e.ID == id);
        }

        public async Task<IActionResult> removeItemFromInv()
        {
            foreach (var item in ShoppingCartController.products)
            {
                var inventory = await _context.Inventory.SingleOrDefaultAsync(m => m.itemID == item.id);
                if (inventory != null)
                {
                    inventory.quantity = inventory.quantity - item.cartQuantity;
                    if (inventory.quantity == 0)
                    {
                        // TODO Alert Sales and the Seller that the inventory is out.
                    }
                    else if (inventory.quantity < 6)
                    {
                        // TODO Alert Sales and the Seller that the inventory is low.
                    }
                    await _context.SaveChangesAsync();
                }
            }
            ShoppingCartController.products.Clear();
            return View("OrderPlaced");
        }

    }
}
