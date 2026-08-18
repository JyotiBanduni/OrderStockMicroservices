using ProductService.Entities;

namespace ProductService.Repository
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);

        Task<Product?> GetByIdAsync(Guid productId);

        Task<List<Product>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync();

        Task UpdateAsync(Product product);

        Task DeleteAsync(Product product);

        Task SaveChangesAsync();

        Task<bool> ReduceStockAsync(
           Guid productId,
           int quantity);
    }
}