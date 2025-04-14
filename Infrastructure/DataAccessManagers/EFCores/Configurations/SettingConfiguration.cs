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
    public class SettingConfiguration : BaseEntityConfiguration<Setting>
    {
        public override void Configure(EntityTypeBuilder<Setting> builder)
        {
            base.Configure(builder);

            //builder.Property(x => x.SizeName)
            //       .IsRequired()
            //       .HasMaxLength(LengthConsts.S);
        }
    }
}
