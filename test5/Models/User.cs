using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace test5.Models
{
    public class User : IdentityUser
    {
        public string OwnerID { get; set; }

        public  string UserType{ get; set; }

        [Required]
        [StringLength(100)]
        public string First { get; set; }

        [Required]
        [StringLength(100)]
        public string Last { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [StringLength(2)]
        public string State { get; set; }

        [Required]
        [StringLength(5)]
        public int Zip { get; set; }

        
        [Phone]
        public string Phone { get; set; }

        public string CardNumber { get; set; }

        [StringLength(4)]
        public string ExpDate { get; set; }

        [StringLength(50)]
        public string NameOnCard { get; set; }

        [StringLength(3)]
        public string Svc { get; set; }


        
    }

    //public enum UserType
    //{
    //    Administrator,
    //    Customer,
    //    Logistics,
    //    Sales
    //}

}
