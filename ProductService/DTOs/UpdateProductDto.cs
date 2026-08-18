namespace ProductService.DTOs
{
    public class UpdateProductDto
    {
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQty { get; set; }

        public bool IsActive { get; set; }
    }
}