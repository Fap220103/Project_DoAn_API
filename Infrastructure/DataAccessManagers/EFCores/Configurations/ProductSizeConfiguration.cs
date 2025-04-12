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
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.HasKey(ps => new { ps.ProductId, ps.SizeId });

            builder.HasOne(ps => ps.Product)  
                    .WithMany(p => p.ProductSize)  
                    .HasForeignKey(ps => ps.ProductId)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ps => ps.Size)
                   .WithMany(s => s.ProductSize)
                   .HasForeignKey(ps => ps.SizeId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
