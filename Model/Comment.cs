using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IShop.Model
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The comment's content can not be empty. ")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DisplayName("content")]
        public string Content { get; set; }

        [ValidateNever]
        [DataType(DataType.Date)]
        public DateTime Published { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ValidateNever]
        public Product Product { get; set; }

        [ValidateNever]
        public string Username { get; set; }

    }
}
