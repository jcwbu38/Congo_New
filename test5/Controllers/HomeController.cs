using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using test5.Models;

namespace test5.Controllers
{
    public class HomeController : Controller
    {
        List<Product> products = new List<Product>() {
            new Product() { image = "/images/image_1.jpg", description = "Angara Round Sapphire Gemstone Stud Earrings in silver", price = 199.95, EntryDate = new DateTime( 2017, 02, 15), itemID = 1 },
            new Product() { image = "/images/image_2.jpg", description = "Dreamland Jewelry Sterling Silver Halo CZ Ring", price = 14.99, EntryDate = new DateTime( 2015, 12, 01), itemID = 2 },
            new Product() { image = "/images/image_3.jpg", description = "Sterling Silver Peridot Diamond Accent Pendant", price = 25.00, EntryDate = new DateTime( 2016, 05, 23), itemID = 3 },
            new Product() { image = "/images/image_4.jpg", description = "Badass Jewelry Sterling Silver Heart Blue Spinel CZ Ring", price = 12.99, discountPrice = 25.99, EntryDate = new DateTime( 2017, 04, 28), itemID = 4},
            new Product() { image = "/images/image_5.jpg", description = ".57 TCW Red Crystal and CZ Necklace 18\"-20\"", price = 23.99, EntryDate = new DateTime( 2016, 11, 15), itemID = 5 },
            new Product() { image = "/images/image_6.jpg", description = "Angara Five Stone Emerald Semi Eternity Ring in White Gold", price = 1989.00, EntryDate = new DateTime( 2017, 07, 01), itemID = 6 },
            new Product() { image = "/images/image_7.jpg", description = "JEULIA Design Heart-shape Infinity Women's Pendent Necklace Round", price = 99.99, EntryDate = new DateTime( 2017, 03, 18), itemID = 7 },
            new Product() { image = "/images/image_8.jpg", description = "Women'sFreshwater Cultured Pearl and White Topaz Pendant", price = 99.99, EntryDate = new DateTime( 2017, 08, 01), itemID = 8 },
            new Product() { image = "/images/image_9.jpg", description = "Round Sapphire Diamond V-Bale Dangling Necklace in 14K White Gold", price = 799.00, EntryDate = new DateTime( 2017, 08, 01), itemID = 9 },
            new Product() { image = "/images/image_10.jpg", description = "Midnight Garden Earring Magical Blue", price = 60.00, EntryDate = new DateTime( 2015, 02, 20), itemID = 10 },
        };

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult ProductDetails( int id )
        {
            foreach ( var product in products)
            {
                if( product.itemID == id )
                {
                    return View(product);
                }
            }

            return Content("nothing found");
        }

        public ActionResult Add(int id, int qty)
        {
            foreach (var product in products)
            {
                if (product.itemID == id)
                {
                    var newCart = new Models.ShoppingCart.Cart() { price = product.price, description = product.description, discountPrice = product.discountPrice, image = product.image, quantity = qty };
                    return RedirectToAction("Index", "ShoppingCart", newCart);
                }
            }
            return Content("You got here!");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
