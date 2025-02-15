using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Infrastructure.SecurityManagers.Navigations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeedManagers.Systems
{
    public class RoleClaimSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public RoleClaimSeeder(RoleManager<IdentityRole> roleManager,DataContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public async Task GenerateDataAsync()
        {
            if (_roleManager.Roles.Any())
            {
                return;
            }
            var adminRole = "Admin";
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
                var role = await _roleManager.FindByNameAsync(adminRole);
                if (role != null)
                {

                    foreach (var item in NavigationBuilder
                        .BuildFinalNavigations()
                        .SelectMany(x => x.Children))
                    {
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", $"{item.Name}:Create"));
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", $"{item.Name}:Read"));
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", $"{item.Name}:Update"));
                        await _roleManager.AddClaimAsync(role, new Claim("Permission", $"{item.Name}:Delete"));
                    }
                }
            }

            var basicRole = "Basic";
            if (!await _roleManager.RoleExistsAsync(basicRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(basicRole));
                var role = await _roleManager.FindByNameAsync(basicRole);
                if (role != null)
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"UserProfile:Create"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"UserProfile:Read"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"UserProfile:Update"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"UserProfile:Delete"));
                }
            }



            var staffRole = "Staff";
            if (!await _roleManager.RoleExistsAsync(staffRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(staffRole));
                var role = await _roleManager.FindByNameAsync(staffRole);
                if (role != null)
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Messenger:Create"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Messenger:Read"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Messenger:Update"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Messenger:Delete"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"OrderList:Create"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"OrderList:Read"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"OrderList:Update"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"OrderList:Delete"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Inventory:Create"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Inventory:Read"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Inventory:Update"));
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", $"Inventory:Delete"));

                }
            }
        }
        public async Task GenerateDataAsync_v2()
        {
            if (await _context.RoleClaims.AnyAsync())
            {
                return;
            }
            var adminRole = "Admin";

            var roleAdmin = await _roleManager.FindByNameAsync(adminRole);
            if (roleAdmin != null)
            {

                foreach (var item in NavigationBuilder
                    .BuildFinalNavigations()
                    .SelectMany(x => x.Children))
                {
                    await _roleManager.AddClaimAsync(roleAdmin, new Claim("Permission", $"{item.Name}:Create"));
                    await _roleManager.AddClaimAsync(roleAdmin, new Claim("Permission", $"{item.Name}:Read"));
                    await _roleManager.AddClaimAsync(roleAdmin, new Claim("Permission", $"{item.Name}:Update"));
                    await _roleManager.AddClaimAsync(roleAdmin, new Claim("Permission", $"{item.Name}:Delete"));
                }
            }


            var basicRole = "Basic";

            var roleBasic = await _roleManager.FindByNameAsync(basicRole);
            if (roleBasic != null)
            {
                await _roleManager.AddClaimAsync(roleBasic, new Claim("Permission", $"UserProfile:Create"));
                await _roleManager.AddClaimAsync(roleBasic, new Claim("Permission", $"UserProfile:Read"));
                await _roleManager.AddClaimAsync(roleBasic, new Claim("Permission", $"UserProfile:Update"));
                await _roleManager.AddClaimAsync(roleBasic, new Claim("Permission", $"UserProfile:Delete"));
            }




            var staffRole = "Staff";

            var roleStaff = await _roleManager.FindByNameAsync(staffRole);
            if (roleStaff != null)
            {
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Messenger:Create"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Messenger:Read"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Messenger:Update"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Messenger:Delete"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"OrderList:Create"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"OrderList:Read"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"OrderList:Update"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"OrderList:Delete"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Inventory:Create"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Inventory:Read"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Inventory:Update"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Inventory:Delete"));

            }

        }
    }
}
