using System.ComponentModel.DataAnnotations;

namespace test5.Models
{
    public class User
    {     
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string First { get; set; }
        [Required]
        [StringLength(100)]
        public string Last { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(2)]
        public string State { get; set; }
        [MaxLength(5)]
        public int Zip { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
