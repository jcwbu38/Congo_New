using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test5.Models
{
    public class Product
    {
        public string image { set; get; }
        public string description { set; get; }
        public double price { set; get; }
        public double discountPrice { set; get; }
        public DateTime EntryDate;
        public int itemID { set; get; }
        // This is a local var to be used for displaying purposes and doesn't need to be in the dB model.
        public int productCount { set; get; }
    }
}