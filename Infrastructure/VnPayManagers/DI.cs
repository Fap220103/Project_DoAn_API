using Application.Services.Externals;
using Infrastructure.PhotoManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.VnPayService
{
    public static class DI
    {
        public static IServiceCollection VnPayManager(this IServiceCollection services, IConfiguration configuration)
        {
   
            services.AddTransient<IVnPayService, VnPayService>();

            return services;
        }
    }
}
