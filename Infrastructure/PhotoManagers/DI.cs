using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PhotoManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterPhotoManager(this IServiceCollection services, IConfiguration configuration)
        {
            var CloudinarySectionName = "CloudinarySettings";
            services.Configure<PhotoSettings>(options => configuration.GetSection(CloudinarySectionName).Bind(options));
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddScoped<PhotoSettings>();
            return services;
        }
    }
}
