using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IShop.ViewModels
{
    public class ProductCreatingWithFileUploadRequest
    {
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [DisplayName("image file")]
        public IFormFile DataFile { get; set; }
        public ProductCreatingRequest Product { get; set; }
    }
}
