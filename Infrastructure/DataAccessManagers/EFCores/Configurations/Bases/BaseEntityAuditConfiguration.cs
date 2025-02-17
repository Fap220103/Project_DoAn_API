using Domain.Bases;
using Domain.Constants;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Configurations.Bases
{
    public abstract class BaseEntityAuditConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntityAudit
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            //BaseEntity
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasMaxLength(IdConsts.MaxLength).IsRequired();

            //BaseEntityAudit
            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .IsRequired(false);

            builder.Property(e => e.CreatedById)
                .IsRequired(false)
                .HasMaxLength(UserIdConsts.MaxLength);

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.UpdatedById)
                .HasMaxLength(UserIdConsts.MaxLength)
                .IsRequired(false);

        }
    }
}
