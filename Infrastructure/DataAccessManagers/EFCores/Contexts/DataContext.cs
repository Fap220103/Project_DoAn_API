using Application.Services.CQS;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations;
using Infrastructure.SecurityManagers.AspNetIdentity;
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
      
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Order> Order { get ; set ; }
        public DbSet<OrderDetail> OrderDetail { get ; set ; }
        public DbSet<Product> Product { get ; set ; }
        public DbSet<ProductCategory> ProductCategory { get ; set ; }
        public DbSet<ProductImage> ProductImage { get ; set ; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Size> Size { get ; set ; }
        public DbSet<Token> Token { get; set; }
  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
            modelBuilder.ApplyConfiguration(new SettingConfiguration());
            modelBuilder.ApplyConfiguration(new SizeConfiguration());
            modelBuilder.ApplyConfiguration(new TokenConfiguration());

            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, Name = "Đỏ", HexCode = "#FF0000" },
                new Color { Id = 2, Name = "Xanh lá", HexCode = "#008000" },
                new Color { Id = 3, Name = "Xanh dương", HexCode = "#0000FF" },
                new Color { Id = 4, Name = "Đen", HexCode = "#000000" },
                new Color { Id = 5, Name = "Trắng", HexCode = "#FFFFFF" },
                new Color { Id = 6, Name = "Xám", HexCode = "#808080" },
                new Color { Id = 7, Name = "Vàng", HexCode = "#FFFF00" },
                new Color { Id = 8, Name = "Cam", HexCode = "#FFA500" },
                new Color { Id = 9, Name = "Tím", HexCode = "#800080" },
                new Color { Id = 10, Name = "Hồng", HexCode = "#FFC0CB" },
                new Color { Id = 11, Name = "Nâu", HexCode = "#A52A2A" },
                new Color { Id = 12, Name = "Lục lam", HexCode = "#00FFFF" }
            );

            modelBuilder.Entity<Size>().HasData(
                new Size { Id = 1, Name = "S" },
                new Size { Id = 2, Name = "M" },
                new Size { Id = 3, Name = "L" },
                new Size { Id = 4, Name = "XL" },
                new Size { Id = 5, Name = "XXL" }
            );
        }
    }
}
