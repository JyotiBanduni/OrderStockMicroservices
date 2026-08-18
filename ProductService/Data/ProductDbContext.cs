using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(
            DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(x => x.ProductId);

                entity.Property(x => x.ProductId)
                    .HasColumnName("ProductId");

                entity.Property(x => x.ProductName)
                    .HasColumnName("ProductName")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.Price)
                    .HasColumnName("Price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(x => x.StockQty)
                    .HasColumnName("StockQty");

                entity.Property(x => x.IsActive)
                    .HasColumnName("IsActive");

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("CreatedAt");

                entity.Property(x => x.UpdatedAt)
                    .HasColumnName("UpdatedAt");
            });
        }
    }
}