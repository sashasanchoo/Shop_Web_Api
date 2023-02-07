using System.ComponentModel.DataAnnotations;

namespace IShop.ViewModels
{
    public class ChangePasswordRequest
    {
        [Required]
        public string OldPassword { get; set; }


        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }


        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match")]
        public string ConfirmPassword { get; set; }
    }
}