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
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {

            builder.Property(x=> x.Name)
                   .IsRequired()
                   .HasMaxLength(LengthConsts.S);
        }
    }
}
