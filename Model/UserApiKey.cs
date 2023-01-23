using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IShop.Model
{
    [Index(nameof(Value), IsUnique = true)]
    public class UserApiKey
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        [JsonIgnore]
        [Required]
        public string UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
