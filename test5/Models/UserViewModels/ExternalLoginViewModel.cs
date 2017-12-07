using System.ComponentModel.DataAnnotations;

namespace test5.Models.UserViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
