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
                products.Add(newCart);
            }
            return View(products);
        }

        public ActionResult New()
        {
            return View();
        }
    }
}