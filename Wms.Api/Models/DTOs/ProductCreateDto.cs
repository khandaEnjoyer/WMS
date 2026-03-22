namespace Wms.Api.Models.DTOs
{
    public class ProductCreateDto
    {
      
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}