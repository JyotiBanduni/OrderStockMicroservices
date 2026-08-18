namespace OrderService.DTOs
{
    public class ProductStockResponseDto
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int StockQty { get; set; }

        public bool IsActive { get; set; }
    }
}