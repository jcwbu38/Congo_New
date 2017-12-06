using System;

namespace test5.Models
{
    public class ShoppingCart
    {
        public string image { set; get; }
        public string description { set; get; }
        public double price { set; get; }
        public double discountPrice { set; get; }
        public DateTime EntryDate { get; }
        public int inventoryQuantity { set; get; }
        public int cartQuantity { set; get; }
        public int id { set; get; }
        public string discountCode { set; get; }
        public string stowLocation { get; set; }
        public string productName { get; set; }
    }
}