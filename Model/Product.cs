using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductsAPI.Model
{
    [Table("Products")]
    public class Product 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ProductTypeId")]
        [Required]
        public int ProductTypeId { get; set; }

        [JsonIgnore]
        public ProductType? ProductType { get; set; }
    }
}