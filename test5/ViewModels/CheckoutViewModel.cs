using test5.Models;
using System.Collections.Generic;


namespace test5.ViewModels
{
    public class CheckoutViewModel
    {
        public User User { get; set; }
        public User ShippingInfo { get; set; }
        public List<ShoppingCart> ShoppingCart { get; set; }
    }
}
