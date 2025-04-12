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
    public class ProductConfiguration : BaseEntityAdvanceConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(o => o.OriginalPrice)
                   .HasPrecision(18, 2);

            builder.Property(o => o.Price)
                   .HasPrecision(18, 2);

            builder.Property(o => o.PriceSale)
                    .HasPrecision(18, 2);

            builder.Property(x => x.Alias)
                .HasMaxLength(NameConsts.MaxLength);

            builder.Property(x => x.ProductCode)
                .IsRequired()
                .HasMaxLength(LengthConsts.S);

            builder.Property(x => x.Image)
                .IsRequired()
                .HasMaxLength(LengthConsts.M);

            builder.HasOne(pc => pc.ProductCategory)
                     .WithMany(p => p.Products)
                     .HasForeignKey(p => p.ProductCategoryId)
                     .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Alias).HasDatabaseName("IX_Product_Alias");

        }
    }
}
