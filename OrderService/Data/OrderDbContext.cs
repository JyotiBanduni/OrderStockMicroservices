using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(
            DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.HasKey(x => x.OrderId);

                entity.Property(x => x.OrderId)
                    .HasColumnName("OrderId");

                entity.Property(x => x.ProductId)
                    .HasColumnName("ProductId");

                entity.Property(x => x.Quantity)
                    .HasColumnName("Quantity");

                entity.Property(x => x.OrderStatus)
                    .HasColumnName("OrderStatus")
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasColumnName("CreatedAt");
            });
        }
    }
}