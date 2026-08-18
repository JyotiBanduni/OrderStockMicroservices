using OrderService.DTOs;

namespace OrderService.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateAsync(
            CreateOrderDto dto);

        Task<OrderResponseDto?> GetByIdAsync(
            Guid orderId);

        Task<(List<OrderResponseDto> Orders,
            int TotalCount)> GetAllAsync(
                int pageNumber,
                int pageSize);
    }
}