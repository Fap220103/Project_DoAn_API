using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class OrderConfiguration : BaseEntityAuditConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);

            builder.Property(o => o.TotalAmount)
                   .HasPrecision(18, 2);

            builder.Property(x => x.CustomerName)
                .IsRequired();

            builder.Property(x => x.Email)
              .IsRequired();

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(LengthConsts.S);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(LengthConsts.M);

            builder.HasIndex(e => e.Code).HasDatabaseName("IX_Order_Code");

        }
    }
}
