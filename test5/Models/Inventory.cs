using System;
namespace test5.Models
{
    public class Inventory
    {
        public int ID { get; set; }
        public int itemID { get; set; }
        public string itemName { get; set; }
        public DateTime dateReceived { get; set; }
        public string locationID { get; set; }
        public string sellerName { get; set; }
        public string image { set; get; }
        public string description { set; get; }
        public string detailedDescription { set; get; }
        public double price { set; get; }
        public int discountPrice { set; get; }
        public int quantity { set; get; }
        public DateTime EntryDate;

    }

}
