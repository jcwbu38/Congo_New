using System;
namespace test5.Models
{
    public class Inventory
    {
        public int ID { get; set;  }
        public int itemID { get; set; }
        public string itemName { get; set; }
        public string dateReceived { get; set; }
        public string locationID { get; set;  }
        public string sellerName { get; set; }
        public string image { set; get; }
        public string description { set; get; }
        public string detailedDescription { set; get; }
        public string price { set; get; }
        public string discountPrice { set; get; }
        public DateTime entryDate;

    }
}
