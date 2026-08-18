using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Entities;

namespace ProductService.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _context.Products.AddAsync(product);

            return product;
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<List<Product>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(x => x.ProductName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Products.CountAsync();
        }

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);

            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ReduceStockAsync(
                    Guid productId,
                    int quantity)
        {
            var rowsAffected =
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"
            UPDATE Products
            SET StockQty = StockQty - {quantity},
                UpdatedAt = GETUTCDATE()
            WHERE ProductId = {productId}
              AND StockQty >= {quantity}
              AND IsActive = 1");

            return rowsAffected > 0;
        }
    }
}