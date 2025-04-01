using Application;
using AspNetCoreRateLimit;
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
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            //>>> Infrastructure Layer
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //>>> Application Layer
            builder.Services.AddApplicationServices();

            builder.Services.RegisterExceptionManagers();

            //>>>  Thêm CORS vào dịch vụ
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200") // Angular chạy trên cổng 4200
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });


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

            // Sử dụng CORS trước khi định tuyến API
            app.UseCors(MyAllowSpecificOrigins);

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
            app.UseSession();
            app.MapControllers();

            app.Run();
        }
    }
}