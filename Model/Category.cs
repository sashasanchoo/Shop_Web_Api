using System.ComponentModel.DataAnnotations;

namespace IShop.Model
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category's name can not be empty")]
        [MinLength(3, ErrorMessage = "Category's name must be from 3 to 30 symbols")]
        [MaxLength(30, ErrorMessage = "Category's name must be from 3 to 30 symbols")]
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
