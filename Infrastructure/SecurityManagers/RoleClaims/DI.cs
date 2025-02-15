using Application.Services.Externals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.RoleClaims
{
    public static class DI
    {
        public static IServiceCollection RegisterPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRoleClaimService, RoleClaimService>();
            return services;
        }
    }
}
