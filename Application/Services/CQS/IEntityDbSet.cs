using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.CQS
{
    public interface IEntityDbSet
    {
        public DbSet<Color> Color { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<ReviewProduct> ReviewProduct { get; set; }
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<UserDiscount> UserDiscount { get; set; }
        
    }
}
