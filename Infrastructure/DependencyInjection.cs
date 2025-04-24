using Infrastructure.CartManagers;
using Infrastructure.CommonManagers;
using Infrastructure.DataAccessManagers.EFCores;
using Infrastructure.EmailManagers;
using Infrastructure.EncryptionManagers;
using Infrastructure.ExcelExportManagers;
using Infrastructure.LoggingManagers.Serilogs;
using Infrastructure.PhotoManagers;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Infrastructure.SecurityManagers.Navigations;
using Infrastructure.SecurityManagers.RoleClaims;
using Infrastructure.SecurityManagers.Tokens;
using Infrastructure.VnPayService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //>>> DataAccess
            services.RegisterDataAccess(configuration);

            //>>> AspNetIdentity
            services.RegisterAspNetIdentity(configuration);

            //>>> Policy
            services.RegisterPolicy(configuration);

            ////>>> Serilog
            services.RegisterSerilog(configuration);

            ////>>> RegisterImageManager
            //services.RegisterImageManager(configuration);

            ////>>> RegisterDocumentManager
            //services.RegisterDocumentManager(configuration);

            //>>> RegisterToken
            services.RegisterToken(configuration);

            //>>> NavigationManager
            services.RegisterNavigationManager(configuration);

            ////>>> NumberSequenceManager
            //services.RegisterNumberSequenceManager(configuration);

            //>>> EmailManager
            services.RegisterEmailManager(configuration);

            //>>> EncryptionManager
            services.RegisterEncryptionManager(configuration);

            //>>> CommonManager
            services.RegisterCommonManager(configuration);

            //>>> PhotoManager
            services.RegisterPhotoManager(configuration);

            //>>> CartManager
            services.RegisterCartManager(configuration);

            //>>> ExcelExportManager
            services.RegisterExcelExportManager(configuration);

            //>>> VnPayManager
            services.VnPayManager(configuration);
            return services;
        }
    }
}
