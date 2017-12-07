using System.ComponentModel.DataAnnotations;

namespace test5.Models.UserViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
