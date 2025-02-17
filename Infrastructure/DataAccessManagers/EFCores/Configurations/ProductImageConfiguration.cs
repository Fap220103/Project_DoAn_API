using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {

            builder.HasOne(ps => ps.Product)
                    .WithMany(p => p.ProductImage)
                    .HasForeignKey(od => od.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
