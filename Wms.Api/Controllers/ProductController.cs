using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wms.Api.Data;
using Wms.Api.Models;

namespace Wms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        
        public ProductController(WarehouseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? sortBy)
        {
            IQueryable<Product> query = _context.Products;
            query = sortBy?.ToLower() switch
            {
                "name" => query.OrderBy(p => p.Name),
                "price" => query.OrderBy(p => p.Price),
                "quantity" => query.OrderByDescending(p => p.Quantity),
                _ => query.OrderBy(p => p.Id)
            };
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
            {
                return NotFound($"Product with ID: {id} not found.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
