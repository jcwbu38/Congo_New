using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test5.Models.ShoppingCart
{
    public class Cart
    {
        public string image { set; get; }
        public string description { set; get; }
        public string price { set; get; }
        public string discountPrice { set; get; }
        public DateTime EntryDate;
    }
}