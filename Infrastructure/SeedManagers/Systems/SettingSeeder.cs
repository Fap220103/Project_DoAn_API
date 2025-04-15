using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Entities;
using Infrastructure.EmailManagers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SeedManagers.Systems
{
    public class SettingSeeder
    {
        private readonly IBaseCommandRepository<Setting> _setting;
        private readonly IUnitOfWork _unitOfWork;

        public SettingSeeder(
            IBaseCommandRepository<Setting> setting,
            IUnitOfWork unitOfWork)
        {
            _setting = setting;
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateDataAsync()
        {
            var settings = new List<Setting>
            {
                new Setting { Key = "SiteName", Value = "CloShop" },
                new Setting { Key = "SupportEmail", Value = "quynhnn@gmail.com" },
                new Setting { Key = "PhoneNumber", Value = "0333220102" },
                new Setting { Key = "Address", Value = "Tầng 3 - The Garden Shopping Center, Đường Mễ Trì, P. Mỹ Đình 1, Q. Nam Từ Liêm, TP. Hà Nội" }
            };

            foreach (var setting in settings)
            {
                var exists = await _setting.AnyAsync(s => s.Key == setting.Key);
                if (!exists)
                {
                    await _setting.CreateAsync(setting);
                }
            }

            await _unitOfWork.SaveAsync();
        }

    }
}
