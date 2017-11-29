using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test5.Models;

namespace test5.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly PurchaseOrderContext _context;

        public PurchaseOrderController(PurchaseOrderContext context)
        {
            _context = context;
        }

        // GET: PurchaseOrder
        public async Task<IActionResult> Index()
        {
            return View(await _context.PurchaseOrder.ToListAsync());
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                .SingleOrDefaultAsync(m => m.ID == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PurchaseOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,userID,firstName,lastName,address1,address2,city,state,zip,email,productID,productName,datePurchased,shipDate,stowLocation")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder.SingleOrDefaultAsync(m => m.ID == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,userID,firstName,lastName,address1,address2,city,state,zip,email,productID,productName,datePurchased,shipDate,stowLocation")] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderExists(purchaseOrder.ID))
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
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                .SingleOrDefaultAsync(m => m.ID == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseOrder = await _context.PurchaseOrder.SingleOrDefaultAsync(m => m.ID == id);
            _context.PurchaseOrder.Remove(purchaseOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrder.Any(e => e.ID == id);
        }
    }
}
