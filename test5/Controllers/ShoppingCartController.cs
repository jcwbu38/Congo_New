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
        public ActionResult Index()
        {
            var products = new List<Cart>() {
                new Cart() { image = "/images/image_1.jpg", description = "Angara Round Sapphire Gemstone Stud Earrings in silver", price = "$199.95", EntryDate = new DateTime( 2017, 02, 15) },
                new Cart() { image = "/images/image_8.jpg", description = "Women'sFreshwater Cultured Pearl and White Topaz Pendant", price = "$99.99", EntryDate = new DateTime( 2017, 08, 01) },
                new Cart() { image = "/images/image_9.jpg", description = "Round Sapphire Diamond V-Bale Dangling Necklace in 14K White Gold", price = "$799.00", EntryDate = new DateTime( 2017, 08, 01) },
                new Cart() { image = "/images/image_10.jpg", description = "Midnight Garden Earring Magical Blue", price = "$60.00", EntryDate = new DateTime( 2015, 02, 20) },
            };
            return View(products);
        }

        public ActionResult New()
        {
            return View();
        }
    }
}