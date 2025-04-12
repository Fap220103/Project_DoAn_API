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
    public class ProductCategoryConfiguration : BaseEntityAdvanceConfiguration<ProductCategory>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ParentId)
                    .IsRequired(false);


            builder.Property(x => x.Alias)
                .HasMaxLength(NameConsts.MaxLength);

            builder.Property(x => x.Icon)
                .HasMaxLength(NameConsts.MaxLength);

            builder.HasOne(pc => pc.ParentCategory)
                  .WithMany(pc => pc.ChildCategories)
                  .HasForeignKey(pc => pc.ParentId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
