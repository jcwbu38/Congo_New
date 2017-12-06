using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace test5.Models
{
    public class User : IdentityUser
    {
        public string UserType { get; set; }

        [StringLength(100)]
        public string First { get; set; }

        
        [StringLength(100)]
        public string Last { get; set; }
        
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        

        public string City { get; set; }

        
        [StringLength(2)]
        public string State { get; set; }
        
        public int Zip { get; set; }

        
        [Phone]
        public string Phone { get; set; }

        [CreditCard]
        public string CardNumber { get; set; }

        [StringLength(4)]
        public string ExpDate { get; set; }

        [StringLength(50)]
        public string NameOnCard { get; set; }

        [StringLength(3)]
        public string Svc { get; set; }


        
    }
}
