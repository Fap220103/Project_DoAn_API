using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CartManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterCartManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICartService, CartService>();
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisConfig = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(redisConfig);
            });
            return services;
        }
    }
}
    