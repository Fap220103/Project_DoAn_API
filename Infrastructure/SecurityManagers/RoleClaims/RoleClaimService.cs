using Application.Services.Externals;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.RoleClaims
{
    public class RoleClaimService : IRoleClaimService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleClaimService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<Claim>> GetClaimListByUserAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new RoleClaimException($"User not found. ID: {userId}");
            }

            List<Claim> userClaims = (await _userManager.GetClaimsAsync(user)).ToList();

            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                var roleIdentity = await _roleManager.FindByNameAsync(role);
                if (roleIdentity != null)
                {
                    var claimsForRole = await _roleManager.GetClaimsAsync(roleIdentity);
                    roleClaims.AddRange(claimsForRole.Select(roleClaim => new Claim(roleClaim.Type, roleClaim.Value)));
                }
            }

            var allClaims = userClaims.Concat(roleClaims)
                                      .Distinct(new ClaimComparer())
                                      .ToList();

            cancellationToken.ThrowIfCancellationRequested();

            return allClaims;
        }

        public async Task<List<Claim>> GetClaimListAsync(
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = _userManager.Users.ToList();
            List<Claim> claims = new List<Claim>();
            foreach (var user in users)
            {
                var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
                claims.AddRange(userClaims);

                var roles = await _userManager.GetRolesAsync(user);
                var roleClaims = new List<Claim>();
                foreach (var role in roles)
                {
                    var roleIdentity = await _roleManager.FindByNameAsync(role);
                    if (roleIdentity != null)
                    {
                        var claimsForRole = await _roleManager.GetClaimsAsync(roleIdentity);
                        roleClaims.AddRange(claimsForRole.Select(roleClaim => new Claim(roleClaim.Type, roleClaim.Value)));
                    }
                }
                claims.AddRange(roleClaims);
            }

            var allClaims = claims
                .Distinct(new ClaimComparer())
                .ToList();

            cancellationToken.ThrowIfCancellationRequested();

            return allClaims;
        }

    }

    public class ClaimComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim? x, Claim? y)
        {
            if (x == null || y == null)
                return false;

            return x.Type == y.Type && x.Value == y.Value;
        }

        public int GetHashCode(Claim obj)
        {
            return (obj.Type! + obj.Value!).GetHashCode();
        }
    }
}
