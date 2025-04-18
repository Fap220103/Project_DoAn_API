using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.AspNetIdentity
{
    public class ApplicationUser : IdentityUser
    {
   
        public string? ProfilePictureName { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ApplicationUser(
            string email         
            )
        {
            EmailConfirmed = true;
            IsBlocked = false;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
            Email = email.Trim();
            UserName = Email;         
        }

    }
}
