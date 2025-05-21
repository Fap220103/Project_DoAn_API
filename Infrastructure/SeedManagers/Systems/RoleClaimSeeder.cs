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
            var managerRole = "Manager";
            if (!await _roleManager.RoleExistsAsync(managerRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(managerRole));
                
            }

            var customerRole = "Customer";
            if (!await _roleManager.RoleExistsAsync(customerRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(customerRole));
             
            }

            var staffRole = "Staff";
            if (!await _roleManager.RoleExistsAsync(staffRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(staffRole));             
               
            }
        }
        public async Task GenerateDataAsync_v2()
        {
            if (await _context.RoleClaims.AnyAsync())
            {
                return;
            }
            var managerRole = "Manager";
            var roleManager = await _roleManager.FindByNameAsync(managerRole);
            if (roleManager != null)
            {

                foreach (var item in NavigationBuilder
                    .BuildFinalNavigations()
                    .SelectMany(x => x.Children))
                {
                    await _roleManager.AddClaimAsync(roleManager, new Claim("Permission", $"{item.Name}:Create"));
                    await _roleManager.AddClaimAsync(roleManager, new Claim("Permission", $"{item.Name}:Read"));
                    await _roleManager.AddClaimAsync(roleManager, new Claim("Permission", $"{item.Name}:Update"));
                    await _roleManager.AddClaimAsync(roleManager, new Claim("Permission", $"{item.Name}:Delete"));
                }
            }

            var staffRole = "Staff";
            var roleStaff = await _roleManager.FindByNameAsync(staffRole);
            if (roleStaff != null)
            {
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Product:Read"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"ProductVariant:Create"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"ProductVariant:Read"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"ProductVariant:Update"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"ProductVariant:Delete"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Order:Create"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Order:Read"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Order:Update"));
                await _roleManager.AddClaimAsync(roleStaff, new Claim("Permission", $"Order:Delete"));
            }

        }
    }
}
