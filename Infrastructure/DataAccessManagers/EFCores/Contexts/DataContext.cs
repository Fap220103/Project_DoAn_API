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
        
        public DbSet<Config> Config { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Color> Color { get ; set ; }
        public DbSet<Inventory> Inventory { get ; set ; }
        public DbSet<Order> Order { get ; set ; }
        public DbSet<OrderDetail> OrderDetail { get ; set ; }
        public DbSet<Product> Product { get ; set ; }
        public DbSet<ProductCategory> ProductCategory { get ; set ; }
        public DbSet<ProductColor> ProductColor { get ; set ; }
        public DbSet<ProductImage> ProductImage { get ; set ; }
        public DbSet<ProductSize> ProductSize { get ; set ; }
        public DbSet<Size> Size { get ; set ; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new TokenConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigConfiguration());
            //modelBuilder.ApplyConfiguration(new ColorConfiguration());
            //modelBuilder.ApplyConfiguration(new TokenConfiguration());
            //modelBuilder.ApplyConfiguration(new ConfigConfiguration());
            //modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            //modelBuilder.ApplyConfiguration(new TokenConfiguration());
            //modelBuilder.ApplyConfiguration(new ConfigConfiguration());
            //modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            //modelBuilder.ApplyConfiguration(new TokenConfiguration());
            //modelBuilder.ApplyConfiguration(new ConfigConfiguration());
            //modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
            //modelBuilder.ApplyConfiguration(new TokenConfiguration());
            //modelBuilder.ApplyConfiguration(new ConfigConfiguration());
        }
    }
}
