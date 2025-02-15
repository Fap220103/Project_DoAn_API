using Application.Services.Externals;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Infrastructure.SecurityManagers.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterEmailManager(this IServiceCollection services, IConfiguration configuration)
        {
            var sendMailSectionName = "SendMail";
            services.Configure<EmailSettings>(options => configuration.GetSection(sendMailSectionName).Bind(options));
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<EmailSettings>();
            return services;
        }
    }
}
