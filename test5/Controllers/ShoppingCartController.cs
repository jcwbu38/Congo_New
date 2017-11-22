using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test5.Models.ShoppingCart;

namespace test5.Controllers
{
    public class ShoppingCartController : Controller
    {
        public static List<Models.ShoppingCart.Cart> products = new List<Models.ShoppingCart.Cart>();
        private static Dictionary<string, double> discountCodes = new Dictionary<string, double>() { { "Cody", 0.65 }, { "Derek", 0.60 }, { "Jon", 0.50 } };



        public ActionResult Index( Cart newCart)
        {
            if (newCart.price > 0)
            {
                foreach( var item in products)
                {
                    if (newCart.id == item.id)
                        return View(products);
                }
                products.Add(newCart);
            }
            return View(products);
        }

        public IActionResult Update(int id, int qty)
        {

            foreach (var item in products)
            {
                if (id == item.id)
                {
                    if (qty < 1) // requirement 3.5.1.1 
                        products.Remove(item);
                    else if ( qty <= item.inventoryQuantity )
                        item.cartQuantity = qty;

                    return View("Index", products);
                }
            }

            return View("Index", products);
        }

        // requirement 3.2.9.3.1
        public IActionResult Discount(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Content("Error, discount code is invalid");

            foreach (var code in discountCodes)
            {
                if (id.Equals(code.Key)) //TODO check code for validity
                {
                    foreach (var item in products)
                    {
                        item.discountPrice = item.price * code.Value;
                        item.discountCode = code.Key;
                        return View("Index", products);
                    }
                }
            }
            return Content("Error, discount code is invalid");
        }

        public IActionResult Checkout()
        {
            return View(products);
        }
    }
}