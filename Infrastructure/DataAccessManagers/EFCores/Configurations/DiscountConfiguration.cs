using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class DiscountConfiguration : BaseEntityConfiguration<Discount>
    {
        public override void Configure(EntityTypeBuilder<Discount> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(NameConsts.MaxLength);

            builder.Property(e => e.Description)
                .HasMaxLength(DescriptionConsts.MaxLength);

        }
    }
}
