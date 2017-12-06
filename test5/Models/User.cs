using System.ComponentModel.DataAnnotations;

namespace test5.Models
{
    public class User
    {     
        public int Id { get; set; }

        public string UserType { get; set; }

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
        public int Zip { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

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
