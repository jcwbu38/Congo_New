using System;
namespace test5.Models
{
    public class PurchaseOrder
    {
        public int ID { get; set; }   
        public int userID { get; set; }   
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int zip { get; set; }
        public string email { get; set; }
        public string productID { get; set; }
        public string productName { get; set; }
        public DateTime datePurchased { get; set; }
        public DateTime shipDate { get; set; }
        public string stowLocation { get; set; }
    }
}
