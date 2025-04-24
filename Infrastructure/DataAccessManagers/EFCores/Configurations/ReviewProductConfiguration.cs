using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class ReviewProductConfiguration : BaseEntityConfiguration<ReviewProduct>
    {
        public override void Configure(EntityTypeBuilder<ReviewProduct> builder)
        {
            base.Configure(builder);

            builder.HasOne(ps => ps.Product)
               .WithMany(p => p.ReviewProducts)
               .HasForeignKey(ps => ps.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
