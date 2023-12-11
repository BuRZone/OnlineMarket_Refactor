using Microsoft.EntityFrameworkCore;
using OnlineMarket.Infrastructure.Entity;

namespace OnlineMarket.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Basket>(entity =>
            {
                entity.ToTable("Basket");

                entity.HasIndex(e => e.UserId, "IX_Basket_UserId").IsUnique();

                entity.HasOne(d => d.User).WithOne(p => p.Basket).HasForeignKey<Basket>(d => d.UserId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasIndex(e => e.BasketId, "IX_Order_BasketId");

                entity.HasIndex(e => e.ProductId, "IX_Order_ProductId");

                entity.HasOne(d => d.Basket).WithMany(p => p.Orders).HasForeignKey(d => d.BasketId);

                entity.HasOne(d => d.Product).WithMany(p => p.Orders).HasForeignKey(d => d.ProductId);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.HasIndex(e => e.SellerId, "IX_Payment_SellerId");

                entity.Property(e => e.PaymentPrice).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Seller).WithMany(p => p.Payments).HasForeignKey(d => d.SellerId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.CategoryId, "IX_Product_CategoryId");

                entity.HasIndex(e => e.SellerId, "IX_Product_SellerId");

                entity.HasIndex(e => e.SubCategoryId, "IX_Product_SubCategoryId");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Seller).WithMany(p => p.Products).HasForeignKey(d => d.SellerId);

                entity.HasOne(d => d.SubCategory).WithMany(p => p.Products).HasForeignKey(d => d.SubCategoryId);
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.ToTable("Seller");

                entity.HasIndex(e => e.CategoryId, "IX_Seller_CategoryId");

                entity.HasIndex(e => e.UserId, "IX_Seller_UserId").IsUnique();

                entity.HasOne(d => d.Category).WithMany(p => p.Sellers).HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.User).WithOne(p => p.Seller).HasForeignKey<Seller>(d => d.UserId);

                entity.HasMany(d => d.SubCategories).WithMany(p => p.Sellers)
                    .UsingEntity<Dictionary<string, object>>(
                        "SellerSubCategory",
                        r => r.HasOne<SubCategory>().WithMany().HasForeignKey("SubCategoryId"),
                        l => l.HasOne<Seller>().WithMany().HasForeignKey("SellerId"),
                        j =>
                        {
                            j.HasKey("SellerId", "SubCategoryId");
                            j.ToTable("SellerSubCategory");
                            j.HasIndex(new[] { "SubCategoryId" }, "IX_SellerSubCategory_SubCategoryId");
                        });
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("SubCategory");

                entity.HasIndex(e => e.CategoryId, "IX_SubCategory_CategoryId");

                entity.HasOne(d => d.Category).WithMany(p => p.SubCategories).HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Purchase).HasColumnType("decimal(18, 2)");
            });

        }

    }
}
