using test5.Models;
using System.Collections.Generic;
using test5.Models.ShoppingCart;


namespace test5.ViewModels
{
    public class CheckoutViewModel
    {
        public User User { get; set; }
        public List<ShoppingCart> ShoppingCart { get; set; }
    }
}
