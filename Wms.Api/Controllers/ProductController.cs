using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wms.Api.Data;
using Wms.Api.Models;
using Wms.Api.Models.DTOs;

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
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Sku = dto.Sku,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            product.Status = product.Quantity > 0
                ? ProductStatus.InStock
                : ProductStatus.OutOfStock;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id) return BadRequest("ID не совпадают");

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // Обновляем поля
            product.Name = updatedProduct.Name;
            product.Quantity = updatedProduct.Quantity;
            product.Price = updatedProduct.Price;
            product.Sku = updatedProduct.Sku;

            // Снова применяем логику статуса!
            UpdateProductStatus(product);

            await _context.SaveChangesAsync();
            return NoContent();
        }
        private void UpdateProductStatus(Product product)
        {
            product.Status = product.Quantity > 0
                ? ProductStatus.InStock
                : ProductStatus.OutOfStock;
        }
    }
}
