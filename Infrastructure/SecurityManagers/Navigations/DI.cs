using Application.Services.Externals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Navigations
{
    public static class DI
    {
        public static IServiceCollection RegisterNavigationManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INavigationService, NavigationService>();

            return services;
        }
    }
}
