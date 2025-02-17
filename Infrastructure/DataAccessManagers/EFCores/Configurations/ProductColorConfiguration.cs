using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.HasKey(ps => new { ps.ProductId, ps.ColorId });

            builder.HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductColor)
                    .HasForeignKey(ps => ps.ProductId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ps => ps.Color)
                   .WithMany(s => s.ProductColor)
                   .HasForeignKey(ps => ps.ColorId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
