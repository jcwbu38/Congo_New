using System;
namespace test5.Models
{
    public class PurchaseOrder
    {
        public int ID { get; set; }
        public string productName { get; set; }
        public int productID { get; set; }
        public DateTime datePurchased { get; set; }
        public string stowLocation { get; set; }
        public DateTime shipDate { get; set; }
        public string customerFirstName { get; set; }
        public string customerLastName { get; set; }
        public int customerID { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string state { get; set; }
        public int zip { get; set; }
        public string email { get; set; }
    }
}
