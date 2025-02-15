using Application.Services.Externals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EncryptionManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterEncryptionManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEncryptionService, EncryptionService>();

            return services;
        }
    }
}
