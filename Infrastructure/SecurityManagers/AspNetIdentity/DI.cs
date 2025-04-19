using Application.Services.Externals;
using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.SecurityManagers.AspNetIdentity
{
    public static class DI
    {
        public static IServiceCollection RegisterAspNetIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var aspNetIdentitySectionName = "AspNetIdentity";
            services.Configure<IdentitySettings>(options => configuration.GetSection(aspNetIdentitySectionName).Bind(options));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                var identitySettings = configuration.GetSection(aspNetIdentitySectionName).Get<IdentitySettings>();
                if (identitySettings == null)
                {
                    throw new IdentityException($"{aspNetIdentitySectionName} configuration section at appsettings.json is missing");
                }

                // Password settings
                options.Password.RequireDigit = identitySettings.Password.RequireDigit;
                options.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
                options.Password.RequireUppercase = identitySettings.Password.RequireUppercase;
                options.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;
                options.Password.RequiredLength = identitySettings.Password.RequiredLength;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.Lockout.DefaultLockoutTimeSpanInMinutes);
                options.Lockout.MaxFailedAccessAttempts = identitySettings.Lockout.MaxFailedAccessAttempts;
                options.Lockout.AllowedForNewUsers = identitySettings.Lockout.AllowedForNewUsers;

                // User settings
                options.User.RequireUniqueEmail = identitySettings.User.RequireUniqueEmail;

                // SignIn settings
                options.SignIn.RequireConfirmedEmail = identitySettings.SignIn.RequireConfirmedEmail;

                // Allowed user name characters
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.AllowedUserNameCharacters += "àáảãạăằắẳẵặâầấẩẫậđèéẻẽẹêềếểễệìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠĂẰẮẲẴẶÂẦẤẨẪẬĐÈÉẺẼẸÊỀẾỂỄỆÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴ ";


            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddDefaultTokenProviders();

            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }

}
