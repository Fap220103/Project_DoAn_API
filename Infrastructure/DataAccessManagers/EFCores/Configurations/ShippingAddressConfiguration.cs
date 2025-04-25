using Domain.Constants;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;
using Infrastructure.SecurityManagers.AspNetIdentity;
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
    public class ShippingAddressConfiguration : BaseEntityConfiguration<ShippingAddress>
    {
        public override void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.AddressLine)
                .HasMaxLength(LengthConsts.M)
                .IsRequired();
            builder.Property(e => e.District)
               .HasMaxLength(LengthConsts.M)
               .IsRequired();
            builder.Property(e => e.Province)
               .HasMaxLength(LengthConsts.M)
               .IsRequired();
            builder.Property(e => e.Ward)
               .HasMaxLength(LengthConsts.M)
               .IsRequired();
            builder.Property(e => e.RecipientName)
               .HasMaxLength(LengthConsts.M)
               .IsRequired();

        }
    }
}
