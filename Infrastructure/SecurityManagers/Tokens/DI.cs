using Infrastructure.EmailManagers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Tokens
{
    public static class DI
    {
        public static IServiceCollection RegisterToken(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSectionName = "Jwt";
            services.Configure<TokenSettings>(options => configuration.GetSection(jwtSectionName).Bind(options));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var tokenSettings = configuration.GetSection(jwtSectionName).Get<TokenSettings>();

                if (tokenSettings?.Key.Length < 32)
                {
                    throw new TokenException("JWT key should be minimal 32 character");
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromMinutes(tokenSettings?.ClockSkewInMinute ?? 5),
                    ValidIssuer = tokenSettings?.Issuer,
                    ValidAudience = tokenSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings?.Key ?? throw new ArgumentNullException("JWT Key is empty")))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                        {
                            context.Token = authorizationHeader.Substring("Bearer ".Length).Trim();
                        }
                        return Task.CompletedTask;
                    }
                };

            });

            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped<TokenSettings>();

            return services;
        }
    }
}
