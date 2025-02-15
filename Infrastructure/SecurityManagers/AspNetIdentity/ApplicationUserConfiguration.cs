using Domain.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.AspNetIdentity
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
           
            builder.Property(u => u.ProfilePictureName)
                .HasMaxLength(NameConsts.MaxLength);

            builder.Property(u => u.IsDeleted)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired(false);

            builder.Property(u => u.CreatedById)
                .HasMaxLength(UserIdConsts.MaxLength);

            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);

            builder.Property(u => u.UpdatedById)
                .HasMaxLength(UserIdConsts.MaxLength);

            builder.HasIndex(u => u.IsDeleted);
        }
    }

}
