using ProductService.DTOs;

namespace ProductService.Service
{
    public interface IProductService
    {
        Task<GetProductDto> CreateAsync(
            CreateProductDto dto);

        Task<GetProductDto?> GetByIdAsync(
            Guid productId);

        Task<(List<GetProductDto> Products, int TotalCount)>
            GetAllAsync(
                int pageNumber,
                int pageSize);

        Task<bool> UpdateAsync(
            Guid productId,
            UpdateProductDto dto);

        Task<bool> DeleteAsync(
            Guid productId);

        Task<bool> ReduceStockAsync(
                Guid productId,
                int quantity);
    }
}