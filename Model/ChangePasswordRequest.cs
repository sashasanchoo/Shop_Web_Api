using System.ComponentModel.DataAnnotations;

namespace IShop.Model
{
    public class ChangePasswordRequest
    {
        [Microsoft.Build.Framework.Required]
        public string OldPassword { get; set; }


        [Microsoft.Build.Framework.Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; }


        [Microsoft.Build.Framework.Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match")]
        public string ConfirmPassword { get; set; }
    }
}
