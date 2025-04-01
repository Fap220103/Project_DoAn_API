using Application.Services.Externals;
using Infrastructure.CommonManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.CartSessionManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterCartSessionManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICartSessionService, CartSessionService>();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thay đổi thời gian timeout nếu cần
                options.Cookie.HttpOnly = true; // Bảo vệ cookie
                options.Cookie.IsEssential = true; // Cookie cần thiết cho ứng dụng
            });
            return services;
        }
    }
}
