using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Infrastructure.ExcelManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExcelExportManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterExcelExportManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IExcelService, ExcelService>();
            return services;
        }
    }
}
