using System.ComponentModel.DataAnnotations;

namespace IShop.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
  
        public Product Product { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "The {0} must be contain only alphabetic characters")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage = "The {0} must be contain only alphabetic characters")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [RegularExpression(@"[a-zA-Z\s\,\:\d\.\-]+", ErrorMessage = "The {0} must be contain only alphabetic characters")]
        public string Address { get; set; }

        [Microsoft.Build.Framework.Required]
        [Phone]
        public string Phone { get; set; }
    }
}
