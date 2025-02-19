using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CommonManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterCommonManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICommonService, CommonService>();
            return services;
        }
    }
}
