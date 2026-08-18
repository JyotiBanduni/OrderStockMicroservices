using ProductService.DTOs;
using ProductService.Entities;
using ProductService.Repository;


namespace ProductService.Service
{
    public class ProductServicee : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductServicee(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<GetProductDto> CreateAsync(
            CreateProductDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
            {
                throw new ArgumentException(
                    "Product name is required.");
            }

            if (dto.Price < 0)
            {
                throw new ArgumentException(
                    "Price cannot be negative.");
            }

            if (dto.StockQty < 0)
            {
                throw new ArgumentException(
                    "Stock quantity cannot be negative.");
            }

            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                ProductName = dto.ProductName,
                Price = dto.Price,
                StockQty = dto.StockQty,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(product);

            await _repository.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<GetProductDto?> GetByIdAsync(
            Guid productId)
        {
            var product =
                await _repository.GetByIdAsync(productId);

            if (product == null)
            {
                return null;
            }

            return MapToDto(product);
        }

        public async Task<(List<GetProductDto> Products,
            int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var products =
                await _repository.GetAllAsync(
                    pageNumber,
                    pageSize);

            var totalCount =
                await _repository.GetTotalCountAsync();

            var result = products
                .Select(MapToDto)
                .ToList();

            return (result, totalCount);
        }

        public async Task<bool> UpdateAsync(
            Guid productId,
            UpdateProductDto dto)
        {
            var product =
                await _repository.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            if (dto.Price < 0)
            {
                throw new ArgumentException(
                    "Price cannot be negative.");
            }

            if (dto.StockQty < 0)
            {
                throw new ArgumentException(
                    "Stock quantity cannot be negative.");
            }

            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.StockQty = dto.StockQty;
            product.IsActive = dto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(product);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid productId)
        {
            var product =
                await _repository.GetByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            await _repository.DeleteAsync(product);

            await _repository.SaveChangesAsync();

            return true;
        }

        private static GetProductDto MapToDto(
            Product product)
        {
            return new  GetProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                StockQty = product.StockQty,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public async Task<bool> ReduceStockAsync(
            Guid productId,
            int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException(
                    "Quantity must be greater than zero.");
            }

            return await _repository.ReduceStockAsync(
                productId,
                quantity);
        }
    }
}