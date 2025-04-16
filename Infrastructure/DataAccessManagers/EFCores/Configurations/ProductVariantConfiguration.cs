using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductVariants)
                    .HasForeignKey(ps => ps.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.Color)
                   .WithMany(s => s.ProductVariants)
                   .HasForeignKey(ps => ps.ColorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ps => ps.Size)
                  .WithMany(s => s.ProductVariants)
                  .HasForeignKey(ps => ps.SizeId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
