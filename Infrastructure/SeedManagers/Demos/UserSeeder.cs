using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeedManagers.Demos
{
    public class UserSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IdentitySettings _identitySettings;

        public UserSeeder(UserManager<ApplicationUser> userManager, IOptions<IdentitySettings> identitySettings)
        {
            _userManager = userManager;
            _identitySettings = identitySettings.Value;
        }
        public async Task GenerateDataAsync()
        {
            if (_userManager.Users.Any()) { return; }
            var adminRole = "Admin";
            var staffRole = "Staff";
            var basicRole = "Basic";

            var users = new List<(string Email, string Password)>
            {
                ("alex.taylor@example.com", "123456"),
                ("jordan.morgan@example.com", "123456"),
                ("taylor.lee@example.com", "123456"),
                ("cameron.drew@example.com", "123456"),
                ("casey.reese@example.com", "123456"),
                ("skyler.morgan@example.com", "123456"),
                ("avery.quinn@example.com", "123456"),
                ("charlie.harper@example.com", "123456"),
                ("jamie.riley@example.com", "123456"),
                ("riley.jordan@example.com", "123456"),
            };

            foreach (var (email, password) in users)
            {
                if (await _userManager.FindByEmailAsync(email) == null)
                {
                    var applicationUser = new ApplicationUser(
                        email,
                        null
                    );

                    applicationUser.EmailConfirmed = true;

                    await _userManager.CreateAsync(applicationUser, password);

                    if (!await _userManager.IsInRoleAsync(applicationUser, staffRole))
                    {
                        await _userManager.AddToRoleAsync(applicationUser, staffRole);
                    }
                    if (!await _userManager.IsInRoleAsync(applicationUser, basicRole))
                    {
                        await _userManager.AddToRoleAsync(applicationUser, basicRole);
                    }
                }
            }

            var adminEmail = _identitySettings.DefaultAdmin.Email;
            var adminPassword = _identitySettings.DefaultAdmin.Password;

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var applicationUser = new ApplicationUser(
                    adminEmail,
                    null
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
