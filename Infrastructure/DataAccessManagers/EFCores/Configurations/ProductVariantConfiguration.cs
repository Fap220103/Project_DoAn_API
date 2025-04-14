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
            

            //builder.HasOne(ps => ps.Product)
            //        .WithMany(p => p.ProductColor)
            //        .HasForeignKey(ps => ps.ProductId)
            //        .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(ps => ps.Color)
            //       .WithMany(s => s.ProductColor)
            //       .HasForeignKey(ps => ps.ColorId)
            //       .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
