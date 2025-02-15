using Application.Services.Externals;
using Infrastructure.EncryptionManagers;
using WebAPI.Common.Handlers;
using WebAPI.Common.Interfaces;

namespace WebAPI.Common
{
    public static class DI
    {
        public static IServiceCollection RegisterExceptionManagers(this IServiceCollection services)
        {
            services.AddTransient<IExceptionHandler, CustomExceptionHandler>();

            return services;
        }
    }
}
