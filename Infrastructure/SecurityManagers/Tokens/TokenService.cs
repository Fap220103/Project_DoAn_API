using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Tokens
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user, List<Claim> userClaims);
        string GenerateRefreshToken();
    }
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;

        public TokenService(
            IOptions<TokenSettings> tokenSettings
            )
        {
            _tokenSettings = tokenSettings.Value;
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var keyBytes = Encoding.UTF8.GetBytes(_tokenSettings.Key);
            return new SymmetricSecurityKey(keyBytes);
        }

        public string GenerateToken(ApplicationUser user, List<Claim> userClaims)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
        };

            claims.AddRange(userClaims);

            var key = GetSymmetricSecurityKey();
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_tokenSettings.ExpireInMinute),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
