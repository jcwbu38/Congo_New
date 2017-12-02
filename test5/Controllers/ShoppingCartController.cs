using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using test5.Models;
using test5.ViewModels;
using System.Threading.Tasks;

namespace test5.Controllers
{
    public class ShoppingCartController : Controller
    {
        public static List<Models.ShoppingCart> products = new List<Models.ShoppingCart>();
        private static Dictionary<string, double> discountCodes = new Dictionary<string, double>() { { "Cody", 0.65 }, { "Derek", 0.60 }, { "Jon", 0.50 } };
        // Need a variable here to store current user

        public ActionResult Index(ShoppingCart newCart)
        {
            if (newCart.price > 0)
            {
                foreach (var item in products)
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
                    else if (qty <= item.inventoryQuantity)
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
                    }
                    return View("Index", products);
                }
            }
            return Content("Error, discount code is invalid");
        }

        // This method handles retreiving and displaying the user and shipping information for the order.
        public IActionResult Checkout()
        {
            if (products.Count() > 0)
            {
                var user = new User
                {
                    Id = 1234,
                    First = "Test",
                    Last = "Account",

                    Address1 = "12438 SE 198th Place",
                    City = "Test",
                    State = "TX",
                    Zip = 98031,
                    Phone = "1234567890",
                    Email = "me@you.com",

                    CardNumber = "1414045612216589",
                    ExpDate = "1215",
                    NameOnCard = "T. Account",
                    Svc = "123"
                };

                var shippingInfo = new User();

                var viewModel = new CheckoutViewModel
                {
                    User = user,
                    ShippingInfo = shippingInfo,
                    ShoppingCart = products
                };
                return View(viewModel);
            }
            else
            {
                return View("Index", products);
            }
        }

        // This method handles displaying the complete order information for the customer to confirm before the order is placed.
        public IActionResult Confirmation(CheckoutViewModel order)
        {
            // Verify the information
            var user = new User
            {
                First = order.User.First,
                Last = order.User.Last,

                //Id = order.User.Id,

                Address1 = order.User.Address1,
                City = order.User.City,
                State = order.User.State,
                Zip = order.User.Zip,
                Phone = order.User.Phone,
                Email = order.User.Email,

                CardNumber = order.User.CardNumber,
                ExpDate = order.User.ExpDate,
                NameOnCard = order.User.NameOnCard,
                Svc = order.User.Svc
            };

            var shippingInfo = new User
            {
                First = order.ShippingInfo.First,
                Last = order.ShippingInfo.Last,
                Address1 = order.ShippingInfo.Address1,
                City = order.ShippingInfo.City,
                State = order.ShippingInfo.State,
                Zip = order.ShippingInfo.Zip,
            };
            var viewModel = new CheckoutViewModel
            {
                User = user,
                ShippingInfo = shippingInfo,
                ShoppingCart = products
            };

            return View(viewModel);
        }
    }
}