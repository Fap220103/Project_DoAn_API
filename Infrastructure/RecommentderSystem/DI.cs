using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RecommentderSystem
{
    public static class DI
    {
        public static IServiceCollection RecommentderSystemManager(this IServiceCollection services, IConfiguration configuration)
        {  
            services.AddTransient<IContentBaseFiltering, ContentBaseFiltering>();
            return services;
        }
    }
}
