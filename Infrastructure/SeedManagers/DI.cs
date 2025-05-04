using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Infrastructure.SeedManagers.Demos;
using Infrastructure.SeedManagers.Systems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeedManagers
{
    public static class DI
    {
        public static IServiceCollection RegisterSystemSeedManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<RoleClaimSeeder>();
            services.AddScoped<SettingSeeder>();
            //services.AddScoped<ConfigSeeder>();
            return services;
        }


        public static IHost SeedSystemData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<DataContext>();
         
            if (!context.Setting.Any())
            {
                var roleClaimSeeder = serviceProvider.GetRequiredService<RoleClaimSeeder>();
                roleClaimSeeder.GenerateDataAsync().GetAwaiter().GetResult();

                var configSeeder = serviceProvider.GetRequiredService<SettingSeeder>();
                configSeeder.GenerateDataAsync().GetAwaiter().GetResult();

                var ClaimSeeder = serviceProvider.GetRequiredService<RoleClaimSeeder>();
                ClaimSeeder.GenerateDataAsync_v2().GetAwaiter().GetResult();
            }
       
            
            return host;
        }

        //>>> Demo Seed

        public static IServiceCollection RegisterDemoSeedManager(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<UserSeeder>();
            return services;
        }
        public static IHost SeedDemoData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var context = serviceProvider.GetRequiredService<DataContext>();
            var userSeeder = serviceProvider.GetRequiredService<UserSeeder>();
            userSeeder.GenerateDataAsync().GetAwaiter().GetResult(); ;
   
            

            return host;
        }
    }
}
