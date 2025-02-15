using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Domain.Constants;
using Infrastructure.DataAccessManagers.EFCores.Configurations.Bases;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations
{
    public class ConfigConfiguration : BaseEntityCommonConfiguration<Config>
    {
        public override void Configure(EntityTypeBuilder<Config> builder)
        {
            base.Configure(builder);


            builder.Property(x => x.SmtpHost)
                .IsRequired()
                .HasMaxLength(NameConsts.MaxLength);

            builder.Property(x => x.SmtpPort)
                .IsRequired();

            builder.Property(x => x.SmtpUserName)
                .IsRequired()
                .HasMaxLength(NameConsts.MaxLength);

            builder.Property(x => x.SmtpPassword)
                .IsRequired()
                .HasMaxLength(PasswordConsts.MaxLength);

            builder.Property(x => x.SmtpUseSSL)
                .IsRequired();

            builder.Property(x => x.Active)
                .IsRequired();


            builder.HasIndex(e => e.SmtpHost).HasDatabaseName("IX_Config_SmtpHost");
            builder.HasIndex(e => e.SmtpUserName).HasDatabaseName("IX_Config_SmtpUserName");

        }
    }
}
