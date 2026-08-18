using OrderService.DTOs;
using OrderService.Entities;
using OrderService.Repositories;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        private readonly IProductServiceClient
            _productServiceClient;

        public OrderService(
            IOrderRepository orderRepository,
            IProductServiceClient productServiceClient)
        {
            _orderRepository = orderRepository;
            _productServiceClient =
                productServiceClient;
        }

        public async Task<OrderResponseDto> CreateAsync(
            CreateOrderDto dto)
        {
            if (dto.ProductId == Guid.Empty)
            {
                throw new ArgumentException(
                    "ProductId is required.");
            }

            if (dto.Quantity <= 0)
            {
                throw new ArgumentException(
                    "Quantity must be greater than zero.");
            }

            // 1. Call Product Service
            var product =
                await _productServiceClient
                    .GetProductAsync(dto.ProductId);

            if (product == null)
            {
                throw new KeyNotFoundException(
                    "Product not found.");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException(
                    "Product is inactive.");
            }

            // 2. Check stock
            if (product.StockQty < dto.Quantity)
            {
                throw new InvalidOperationException(
                    "Insufficient stock.");
            }

            // 3. Atomically reduce stock
            var stockUpdated =
                await _productServiceClient
                    .ReduceStockAsync(
                        dto.ProductId,
                        dto.Quantity);

            if (!stockUpdated)
            {
                throw new InvalidOperationException(
                    "Unable to reduce stock. " +
                    "Stock may have changed.");
            }

            // 4. Create order
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                OrderStatus = "CREATED",
                CreatedAt = DateTime.UtcNow
            };

            await _orderRepository.CreateAsync(order);

            await _orderRepository.SaveChangesAsync();

            return MapToDto(order);
        }

        public async Task<OrderResponseDto?> GetByIdAsync(
            Guid orderId)
        {
            var order =
                await _orderRepository
                    .GetByIdAsync(orderId);

            if (order == null)
            {
                return null;
            }

            return MapToDto(order);
        }

        public async Task<(
            List<OrderResponseDto> Orders,
            int TotalCount)> GetAllAsync(
                int pageNumber,
                int pageSize)
        {
            var orders =
                await _orderRepository.GetAllAsync(
                    pageNumber,
                    pageSize);

            var totalCount =
                await _orderRepository
                    .GetTotalCountAsync();

            var result = orders
                .Select(MapToDto)
                .ToList();

            return (result, totalCount);
        }

        private static OrderResponseDto MapToDto(
            Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                OrderStatus = order.OrderStatus,
                CreatedAt = order.CreatedAt
            };
        }
    }
}