namespace OrderService.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public string OrderStatus { get; set; } = "CREATED";

        public DateTime CreatedAt { get; set; }
    }
}