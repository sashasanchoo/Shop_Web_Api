using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IShop.ViewModels
{
    public class ProductCreatingRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The {0} can not be empty . ")]
        [DisplayName("name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The {0} can not be empty . ")]
        [DataType(DataType.Date)]
        public DateTime Published { get; set; }

        [Required(ErrorMessage = "The {0} can not be empty . ")]
        [DisplayName("image path")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "The {0} can not be empty . ")]
        [DisplayName("content")]
        public string Content { get; set; }
        [Required(ErrorMessage = "The {0} can not be empty . ")]
        [DisplayName("price")]
        public double Price { get; set; }
        [Required(ErrorMessage = "The {0} can not be empty . ")]
        [DisplayName("category name")]
        public string CategoryName { get; set; }
    }
}
