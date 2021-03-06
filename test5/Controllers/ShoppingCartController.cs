﻿/* ShoppingCartController.cs
 * This contoller provides access to the the shopping cart as well accepting the PO information and sending it to the PurchaseOrderController.
 * This file covers design requirements 4.13 - 4.16, 4.14.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using test5.Models;
using test5.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace test5.Controllers
{
    [AllowAnonymous]
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

        // ShoppingCart/Index - this method updates an item in the shopping cart based on the item ID and the quantity. (section 4.14, 4.15)  
        public IActionResult Update(int id, int qty)
        {
            foreach (var item in products)
            {
                if (id == item.id)
                {
                    if (qty < 1) // requirement 3.5.1.1
                        products.Remove(item);
                    else if (qty <= item.inventoryQuantity) // requirement 3.2.9.3.2 
                        item.cartQuantity = qty;

                    return View("Index", products);
                }
            }
            return View("Index", products);
        }

        // ShoppingCart/Index - this method applies a dscount code (if valid) to the items in the shopping cart. (section 4.16, requirement 3.2.9.3.1).
        public IActionResult Discount(string id)
        {
            if (String.IsNullOrEmpty(id))
                return Content("Error, discount code is invalid");
            // requirement 3.2.9.3.1
            foreach (var code in discountCodes)
            {
                if (id.Equals(code.Key)) //TODO check code for validity
                {
                    foreach (var item in products)
                    {
                        if (item.discountPrice == 0)
                        {
                            item.discountPrice = item.price * code.Value;
                            item.discountCode = code.Key;
                        }
                        else
                        {
                            item.discountCode = "Already on clearance.";
                        }
                    }
                    return View("Index", products);
                }
            }
            return Content("Error, discount code is invalid");
        }

        public IActionResult Empty()
        {
            products.Clear();
            return View();
        }

        // ShoppingCart/Checkout - This method handles retreiving and displaying the user and shipping information for the order.

        public IActionResult Checkout(User user)
        {
            if (products.Count() > 0)
            {
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
        

        // ShoppingCart/Confirmation - This method handles displaying the complete order information for the customer to confirm before the order is placed.
        public IActionResult Confirmation(CheckoutViewModel order)
        {
            // Gather the user, shipping, and cart information to display for the customer to confirm.
            var user = new User
            {
                First = order.User.First,
                Last = order.User.Last,

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