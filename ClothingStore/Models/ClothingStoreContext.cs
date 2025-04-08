using ClothingStore.Decorator;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Models
{
    public class ClothingStoreContext : DbContext
    {
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasKey(c => c.IdCate); // Xác định khóa chính

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.IdOrder, od.IdProduct });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.IdOrder);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.IdProduct);

        }

    }
}
