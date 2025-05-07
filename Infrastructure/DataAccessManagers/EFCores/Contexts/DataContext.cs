using Application.Services.CQS;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Contexts
{
    public class DataContext : IdentityDbContext<ApplicationUser>, IEntityDbSet
    {
        public DataContext()
        {
            
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Color> Color { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Order { get ; set ; }
        public DbSet<OrderDetail> OrderDetail { get ; set ; }
        public DbSet<Product> Product { get ; set ; }
        public DbSet<ProductCategory> ProductCategory { get ; set ; }
        public DbSet<ProductImage> ProductImage { get ; set ; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<ReviewProduct> ReviewProduct { get; set; }
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Size> Size { get ; set ; }
        public DbSet<Token> Token { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserDiscount> UserDiscount { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new DiscountConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
            modelBuilder.ApplyConfiguration(new ShippingAddressConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new SizeConfiguration());
            modelBuilder.ApplyConfiguration(new TokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserDiscountConfiguration());

        }
    }
}
