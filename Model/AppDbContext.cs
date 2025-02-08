using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Model
{
     public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        // When creating the database model, seed data is added for initial values.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductType>().HasData(
                new ProductType() { Id = 1, TypeName = "Telefon" },
                new ProductType() { Id = 2, TypeName = "Bilgisayar" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Iphone 12", Price = 25000, IsActive = true, ProductTypeId = 1 },
                new Product() { Id = 2, Name = "Iphone 13", Price = 35000, IsActive = true, ProductTypeId = 1 },
                new Product() { Id = 3, Name = "Iphone 14", Price = 50000, IsActive = true, ProductTypeId = 1 },
                new Product() { Id = 4, Name = "Macbook Pro M3 16GB 256GB", Price = 74000, IsActive = true, ProductTypeId = 2 },
                new Product() { Id = 5, Name = "Samsung S21", Price = 18500, IsActive = true, ProductTypeId = 1 },
                new Product() { Id = 6, Name = "Asus Zenbook 14", Price = 28750, IsActive = true, ProductTypeId = 2 },
                new Product() { Id = 7, Name = "Xiaomi Mi 14", Price = 32000, IsActive = true, ProductTypeId = 1 }
            );
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }
    }
}