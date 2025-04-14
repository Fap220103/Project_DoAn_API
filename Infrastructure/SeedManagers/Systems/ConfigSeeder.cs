//using Application.Services.Externals;
//using Application.Services.Repositories;
//using Domain.Entities;
//using Infrastructure.EmailManagers;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.SeedManagers.Systems
//{
//    public class ConfigSeeder
//    {
//        private readonly IBaseCommandRepository<Config> _config;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IEncryptionService _encryptionService;
//        private readonly EmailSettings _emailSettings;

//        public ConfigSeeder(
//            IBaseCommandRepository<Config> config,
//            IUnitOfWork unitOfWork,
//            IEncryptionService encryptionService,
//            IOptions<EmailSettings> emailSettings)
//        {
//            _config = config;
//            _unitOfWork = unitOfWork;
//            _encryptionService = encryptionService;
//            _emailSettings = emailSettings.Value;
//        }

//        public async Task GenerateDataAsync()
//        {
//            var environments = new List<string>
//            {
//                "Production",
//                "Staging",
//                "Development"
//             };

//            var index = 1;
//            foreach (var environment in environments)
//            {


//                var entity = new Config(
//                    null,
//                    environment,
//                    null,
//                    "smtp.gmail.com",
//                    587,
//                    _emailSettings.Email,
//                    _encryptionService.Encrypt(_emailSettings.Password),
//                    false,
//                    index == 1 ? true : false
//                    );

//                await _config.CreateAsync(entity);
//                index++;
//            }
//            await _unitOfWork.SaveAsync();

//        }
//    }
//}
