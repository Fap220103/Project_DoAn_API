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
        public DbSet<Config> Config { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductColor> ProductColor { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<ProductSize> ProductSize { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Token> Token { get; set; }
    }
}
