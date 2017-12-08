using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace test5.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "First name")]
        public string First { get; set; }

        [StringLength(100)]
        [Display(Name = "Last name")]
        public string Last { get; set; }

        [Display(Name = "Address 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address 2")]
        public string Address2 { get; set; }


        public string City { get; set; }

        [StringLength(2)]
        [Display(Name = "State (two letter abbrev.)")]
        public string State { get; set; }

        public int Zip { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [CreditCard]
        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [StringLength(4)]
        [Display(Name = "Expiration date")]
        public string ExpDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Name on card")]
        public string NameOnCard { get; set; }

        [StringLength(3)]
        [Display(Name = "SVC")]
        public string Svc { get; set; }

        public string StatusMessage { get; set; }
    }
}
