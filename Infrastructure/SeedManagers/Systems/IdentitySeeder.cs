using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeedManagers.Systems
{
    public class IdentitySeeder
    {
        private readonly IdentitySettings _identitySettings;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentitySeeder(
            IOptions<IdentitySettings> identitySettings,
            UserManager<ApplicationUser> userManager)
        {
            _identitySettings = identitySettings.Value;
            _userManager = userManager;
        }
        public async Task GenerateDataAsync()
        {
            
            var adminEmail = _identitySettings.DefaultAdmin.Email;
            var adminPassword = _identitySettings.DefaultAdmin.Password;

            var adminRole = "Admin";
            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var applicationUser = new ApplicationUser(
                    adminEmail
                    );
                applicationUser.UserName = "Admin";
                applicationUser.EmailConfirmed = true;

                //create user Root Admin
                await _userManager.CreateAsync(applicationUser, adminPassword);

                //add Admin role to Root Admin
                if (!await _userManager.IsInRoleAsync(applicationUser, adminRole))
                {
                    await _userManager.AddToRoleAsync(applicationUser, adminRole);
                }

                

            }
        }
    }
}
