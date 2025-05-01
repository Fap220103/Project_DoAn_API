using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Infrastructure.PhotoManagers;
using Infrastructure.VnPayManagers;
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
            var vnPaySectionName = "VNPay";
            services.Configure<VnPaySetting>(options => configuration.GetSection(vnPaySectionName).Bind(options));

            services.AddScoped<VnPaySetting>();
            services.AddTransient<IVnPayService, VnPayService>();

            return services;
        }
    }
}
