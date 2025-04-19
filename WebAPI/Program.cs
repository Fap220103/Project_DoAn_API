using Application;
using AspNetCoreRateLimit;
using Infrastructure;
using Infrastructure.DataAccessManagers.EFCores;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Infrastructure.SeedManagers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
                              .AllowAnyHeader()
                              .AllowCredentials();
                               
                    });
            });


            builder.Services.AddControllers();
         
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PROJECT-DOAN-API", Version = "v1" });

                // Thêm xác thực JWT vào Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token JWT vào đây: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

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

            app.MapControllers();

            app.Run();
        }
    }
}