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
    public class ColorConfiguration : BaseEntityConfiguration<Color>
    {
        public override void Configure(EntityTypeBuilder<Color> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ColorCode)
                   .IsRequired()
                   .HasMaxLength(LengthConsts.S);
            builder.Property(x => x.ColorName)
                  .IsRequired()
                  .HasMaxLength(LengthConsts.S);
        }
    }
}
