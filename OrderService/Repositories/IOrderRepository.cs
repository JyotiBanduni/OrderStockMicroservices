using OrderService.Entities;

namespace OrderService.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);

        Task<Order?> GetByIdAsync(Guid orderId);

        Task<List<Order>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync();

        Task SaveChangesAsync();
    }
}