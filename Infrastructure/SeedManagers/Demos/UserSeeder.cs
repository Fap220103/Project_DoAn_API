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
            var managerRole = "Manager";
            var staffRole = "Staff";
            var customerRole = "Customer";

            var staffUsers = new List<(string Email, string Password)>
            {
                ("staff1@gmail.com", "123456"),
                ("staff2@gmail.com", "123456"),
                ("staff3@gmail.com", "123456"),
            };

            foreach (var (email, password) in staffUsers)
            {
                if (await _userManager.FindByEmailAsync(email) == null)
                {
                    var applicationUser = new ApplicationUser(
                        email
                    );

                    applicationUser.EmailConfirmed = true;

                    await _userManager.CreateAsync(applicationUser, password);

                    if (!await _userManager.IsInRoleAsync(applicationUser, staffRole))
                    {
                        await _userManager.AddToRoleAsync(applicationUser, staffRole);
                    }
                }
            }

            var customerUsers = new List<(string Email, string Password)>
            {
                ("customer1@gmail.com", "123456"),
                ("customer2@gmail.com", "123456"),
                ("customer3@gmail.com", "123456"),
            };

            foreach (var (email, password) in staffUsers)
            {
                if (await _userManager.FindByEmailAsync(email) == null)
                {
                    var applicationUser = new ApplicationUser(
                        email
                    );

                    applicationUser.EmailConfirmed = true;

                    await _userManager.CreateAsync(applicationUser, password);

                    if (!await _userManager.IsInRoleAsync(applicationUser, customerRole))
                    {
                        await _userManager.AddToRoleAsync(applicationUser, customerRole);
                    }
                }
            }

            var adminEmail = _identitySettings.DefaultAdmin.Email;
            var adminPassword = _identitySettings.DefaultAdmin.Password;

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var applicationUser = new ApplicationUser(
                    adminEmail
                    );
                applicationUser.UserName = "Manager";
                applicationUser.EmailConfirmed = true;

                //create user Root Admin
                await _userManager.CreateAsync(applicationUser, adminPassword);

                //add Admin role to Root Admin
                if (!await _userManager.IsInRoleAsync(applicationUser, managerRole))
                {
                    await _userManager.AddToRoleAsync(applicationUser, managerRole);
                }
            }
        }
    }
}
