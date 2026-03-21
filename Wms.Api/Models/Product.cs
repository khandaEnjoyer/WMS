namespace Wms.Api.Models
{
    public enum ProductStatus
    {
        InStock,    
        OutOfStock, 
        Damaged,    
        Discontinued 
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.InStock;
    }
}
