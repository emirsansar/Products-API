using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsAPI.Model
{
    [Table("ProductTypes")]

    public class ProductType 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string TypeName { get; set; } = null!;

        public ICollection<Product>? Products { get; set; }
    }
}