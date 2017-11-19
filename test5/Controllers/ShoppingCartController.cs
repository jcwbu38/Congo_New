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
                    if (qty < 1)
                        products.Remove(item);
                    else
                        item.quantity = qty;

                    return View("Index", products);
                }
            }

            return View("Index", products);
        }

        public IActionResult Checkout()
        {
            return View(products);
        }
    }
}