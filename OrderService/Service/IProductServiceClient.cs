using OrderService.DTOs;

namespace OrderService.Services
{
    public interface IProductServiceClient
    {
        Task<ProductStockResponseDto?> GetProductAsync(
            Guid productId);

        Task<bool> ReduceStockAsync(
            Guid productId,
            int quantity);
    }
}