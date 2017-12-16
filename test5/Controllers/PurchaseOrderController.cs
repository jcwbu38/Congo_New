/* PurchaseOrderController.cs
 * This contoller provides access to the PO database for updating.
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
using test5.ViewModels;

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

            try
            {
                _context.Update(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

        // This method sends the payment infomation to a third party for processing. (section 2.7).
        private int sendCCInfoForPayment(User ccInfo )
        {
            // The code to send the info off for payment would go here.
            return 1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // ShoppingCart/OrderPlaced - this method handles sending the credit card information for payment and creating the PO for the order.
        public async Task<IActionResult> CreatePO(CheckoutViewModel order )
        {
            //Send CC info for payment (section 2.6)
            int paymentSuccess = sendCCInfoForPayment(order.User);

            if (paymentSuccess == 1)
            {
                var poString = await _context.PurchaseOrder.MaxAsync(m => m.poID);
                int poNum;
                if (poString != null)
                {
                    poNum = Int32.Parse(poString) + 1;
                    if (poNum == 1)
                    {
                        poNum = DateTime.Now.Year * 100000;
                    }
                }
                else
                    poNum = DateTime.Now.Year * 100000;

                foreach (var item in ShoppingCartController.products)
                {
                    PurchaseOrder po = new PurchaseOrder
                    {
                        ID = await _context.PurchaseOrder.CountAsync(),
                        //userID = Int32.Parse(order.User.Id), TODO add back when user model has an ID field.
                        firstName = order.ShippingInfo.First,
                        lastName = order.ShippingInfo.Last,
                        address1 = order.ShippingInfo.Address1,
                        address2 = order.ShippingInfo.Address2,
                        city = order.ShippingInfo.City,
                        state = order.ShippingInfo.State,
                        zip = order.ShippingInfo.Zip,
                        email = order.User.Email,
                        phoneNumber = order.User.Phone,

                        productID = item.id.ToString(),
                        productName = item.productName,
                        datePurchased = DateTime.Now,
                        stowLocation = item.stowLocation,
                        quantity = item.cartQuantity,
                        poID = poNum.ToString()
                    };

                    _context.Add(po);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                return View("ProcessingError");
            }

            return RedirectToAction("removeItemFromInv", "Inventory");
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrder.Any(e => e.ID == id);
        }
    }
}
