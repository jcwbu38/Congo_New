/* ReportingController.cs
 * This contoller provides access to the reporting system that will be used by users to view the database.
 *
 */
using System;
using Microsoft.EntityFrameworkCore;
using test5.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test5.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace test5.Controllers
{
    
    public class ReportingController : Controller
    {

        private readonly PurchaseOrderContext _context;

        public ReportingController(PurchaseOrderContext context)
        {
            _context = context;
        }
       
        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            // Get all purchase orders
            var purchaseOrders = from m in _context.PurchaseOrder
                         select m;

            // Select purchase orders that have not been shipped
            var outstandingPO = purchaseOrders.Where(s => s.shipDate.Date > DateTime.Now.Date);
            var shippedPO = purchaseOrders.Where(s => s.shipDate.Date < DateTime.Now.Date);
            var shippingTodayPO = purchaseOrders.Where(s => s.shipDate.Date == DateTime.Now.Date);

            var reportingVM = new ReportingViewModel();
            reportingVM.outstandingPO = await outstandingPO.ToListAsync();
            reportingVM.shippedPO = await shippedPO.ToListAsync();
            reportingVM.shippingTodayPO = await shippingTodayPO.ToListAsync();

            return View(reportingVM);
            //return View(await purchaseOrders.ToListAsync());

        }
    }
}
