using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Model;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context) 
        {
            _context = context;
        }

        // GET: api/products - Fetches all active products.
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.
                                    Products
                                    .Where(i => i.IsActive)
                                    .Include(p => p.ProductType)
                                    .Select(p => ProductToDTO(p))
                                    .ToListAsync();

            if (products == null) return NotFound();

            return Ok(products);
        }

        // GET: api/products/byType/{productTypeId} - Fetch product by its ProductTypeId.
        [HttpGet("byType/{productTypeId}")]
        public async Task<IActionResult> GetProductsByType(int? productTypeId)
        {
            if (productTypeId == null)
                return NotFound(); // StatusCode(404, "");
            
            var products = await _context
                            .Products
                            .Where(p => p.ProductTypeId == productTypeId)
                            .Select(p => ProductToDTO(p))
                            .ToListAsync();

            if (products == null || !products.Any())
                return NotFound();
            
            return Ok(products);
        }

        // GET: api/products/byId/{id} - Fetch product by its Id.
        [HttpGet("byId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id == null)
            {
                return NotFound(); // StatusCode(404, "");
            }
            
            var p = await _context
                            .Products
                            .Where(p => p.Id == id)
                            .Select(p => ProductToDTO(p))
                            .FirstOrDefaultAsync();

            if (p == null)
                return NotFound();
            
            return Ok(p);
        }

        // GET: api/products/byPriceRange - Fetch products by price range.
        [HttpGet("byPriceRange/{minPrice}/{maxPrice}")]
        public async Task<IActionResult> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _context
                                .Products
                                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                                .Select(p => ProductToDTO(p))
                                .ToListAsync();

            if (products == null || !products.Any())
                return NotFound();
            
            return Ok(products);
        }

        // POST: api/products - Creates a new product.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            var productType = await _context.ProductTypes
                                     .FirstOrDefaultAsync(pt => pt.Id == product.ProductTypeId);
                                 
            if (productType == null)
                return NotFound("ProductType not found");  // If product type is not found, return 404.

            // If the ProductType's Products collection is null, initialize it with a new List<Product>.
            // Then, add the new product to the ProductType's Products collection.
            productType.Products ??= new List<Product>();
            productType.Products.Add(product);
                           
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new {id = product.Id}, product); // 201 Code
        }

        // PUT: api/products/{id} - Updates an existing product.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest(); // Error 4..

            var currentProduct = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);

            if (currentProduct == null) return NotFound();

            currentProduct.Name = product.Name;
            currentProduct.Price = product.Price;
            currentProduct.IsActive = product.IsActive;

            try { await _context.SaveChangesAsync(); }
            catch { return NotFound(); }

            return NoContent(); // 204
        }

        // DELETE: api/products/{id} - Deletes a product.
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(int? id) {
            if (id == null) return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);

            try { await _context.SaveChangesAsync();}
            catch { return NotFound(); }

            return NoContent();
        }

        // Converts a Product entity to a ProductDTO.
        private static ProductDTO ProductToDTO(Product p)
        {
            var entity = new ProductDTO();

            if (p != null) 
            {
                entity.Id = p.Id;
                entity.Name = p.Name;
                entity.Price = p.Price;
                //entity.ProductTypeId = p.ProductTypeId;
                entity.Type = p.ProductType?.TypeName ?? "Unknown";
            }

            return entity;
        }

    }
}