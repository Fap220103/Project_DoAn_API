using Application.Services.Externals;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.GeneralReview
{
    public static class DI
    {
        public static IServiceCollection GeneralReviewManager(this IServiceCollection services, IConfiguration configuration)
        {  
            //services.AddTransient<IGeneralReview, GeneralReview>();
            services.AddHttpClient<IGeneralReview, GeneralReview>();

            return services;
        }
    }
}
