using Application;
using Infrastructure;
using Infrastructure.DataAccessManagers.EFCores;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Infrastructure.SeedManagers;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using WebAPI.Common;
using WebAPI.Common.Handlers;
using WebAPI.Common.Middlewares;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //>>> Infrastructure Layer
            builder.Services.AddInfrastructureServices(builder.Configuration);
          
            //>>> Application Layer
            builder.Services.AddApplicationServices();

            builder.Services.RegisterExceptionManagers();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //>>> Register Seeder
            builder.Services.RegisterSystemSeedManager(builder.Configuration);
            builder.Services.RegisterDemoSeedManager(builder.Configuration);
            builder.Services.AddSingleton<Microsoft.AspNetCore.Authentication.ISystemClock, Microsoft.AspNetCore.Authentication.SystemClock>();
            var app = builder.Build();

            //craete database
            app.CreateDatabase();

            //seed database with system data
            app.SeedSystemData();

            //seed database with demo data
            if (app.Configuration.GetValue<bool>("IsDemoVersion"))
            {
                app.SeedDemoData();
            }

            app.UseExceptionHandler(options => { });
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
           

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}