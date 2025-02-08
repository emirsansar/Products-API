using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Model;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductTypeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductTypeController(AppDbContext context) 
        {
            _context = context;
        }

        // GET: api/producttype - Fetches all active product types.
        [HttpGet]
        public async Task<IActionResult> GetTypes()
        {
            var types = await _context.
                                    ProductTypes
                                    .Include(pt => pt.Products)
                                    .ToListAsync();

            if (types == null) return NotFound();

            return Ok(types);
        }
        
        // GET: api/producttype/{id} - Fetch product type by its Id.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var type = await _context
                            .ProductTypes
                            .Where(p => p.Id == id)
                            .Include(pt => pt.Products)
                            .FirstOrDefaultAsync();

            if (type == null) return NotFound();

            return Ok(type);
        }

        // POST: api/producttype - Creates a new product type.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateType(ProductType type)
        {
            _context.ProductTypes.Add(type);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetType), new {id = type.Id}, type);
        }

        // PUT: api/producttype/{id} - Updates an existing product type.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateType(int id, ProductType type)
        {
            if (id != type.Id) return BadRequest();

            var currentType = await _context.ProductTypes.FirstOrDefaultAsync(t => t.Id == id);

            if (currentType == null) return NotFound();

            currentType.TypeName = type.TypeName;

            try 
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/producttype/{id} - Deletes a product type.
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteType(int? id) {
            if (id == null) return NotFound();

            var type = await _context.ProductTypes.FirstOrDefaultAsync(i => i.Id == id);

            if (type == null) return NotFound();

            _context.ProductTypes.Remove(type);

            try { await _context.SaveChangesAsync();}
            catch { return NotFound(); }

            return NoContent();
        }

    }
}