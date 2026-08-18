namespace ProductService.DTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQty { get; set; }
    }
}