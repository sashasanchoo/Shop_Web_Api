using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IShop.Model
{
    public class User:IdentityUser
    {
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"[a-zA-Z0-9._]+", ErrorMessage = "The {0} must contain notning except alphabetic characters, digits, \".\" or \"_\" symbols")]
        [DisplayName("username")]
        public override string UserName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [EmailAddress]
        [Display(Name = "Email")]
        public override string Email { get; set; }
    }
}
