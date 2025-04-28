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
    public class UserDiscountConfiguration : BaseEntityConfiguration<UserDiscount>
    {
        public override void Configure(EntityTypeBuilder<UserDiscount> builder)
        {
            base.Configure(builder);
            builder.HasOne(ud => ud.Discount)
                   .WithMany(d => d.UserDiscounts)
                   .HasForeignKey(ud => ud.DiscountId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
