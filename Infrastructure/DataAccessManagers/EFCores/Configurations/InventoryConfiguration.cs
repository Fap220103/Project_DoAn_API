using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.HasKey(i => i.ProductVariantId);

            builder.HasOne(i => i.ProductVariant)
                .WithOne(pv => pv.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductVariantId);
                //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}
