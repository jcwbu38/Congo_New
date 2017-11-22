using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test5.Models.ShoppingCart
{
    public class Cart
    {
        public string image { set;  get; }
        public string description { set;  get; }
        public double price { set; get; }
        public double discountPrice { set; get; }
        public DateTime EntryDate { get; }
        public int inventoryQuantity { set; get; }

        public int cartQuantity { set; get; }
        public int id { set; get; }
        public string discountCode { set; get;  }
    }
}