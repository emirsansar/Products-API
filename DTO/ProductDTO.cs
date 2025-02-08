using ProductsAPI.Model;

namespace ProductsAPI.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int ProductTypeId { get; set; }

        public string Type { get; set; } = null!;
    }
}