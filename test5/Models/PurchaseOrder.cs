using System;
using System.ComponentModel.DataAnnotations;
namespace test5.Models
{
    public class PurchaseOrder
    {
        public int ID { get; set; }   
        public int userID { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string address1 { get; set; }
        public string address2 { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        [StringLength(2)]
        public string state { get; set; }
        [Required]
        public int zip { get; set; }
        [Required]
        public string email { get; set; }
        public string phoneNumber { get; set; }
        [Required]
        public string productID { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public DateTime datePurchased { get; set; }
        [Required]
        public DateTime shipDate { get; set; }
        [Required]
        public string stowLocation { get; set; }
        [Required]
        public int quantity { get; set; }

    }
}
