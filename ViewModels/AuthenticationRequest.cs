using System.ComponentModel.DataAnnotations;

namespace IShop.ViewModels
{
    public class AuthenticationRequest
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
