//using Application.Services.CQS.Queries;
//using Application.Services.Externals;
//using Application.Services.Repositories;
//using Domain.Entities;
//using FluentValidation;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Features.Configs.Commands
//{
//    public class CreateConfigResult
//    {
//        public string Id { get; init; } = null!;
//        public string Message { get; init; } = null!;
//    }

//    public class CreateConfigRequest : IRequest<CreateConfigResult>
//    {
//        public string? UserId { get; init; } //created by userid
//        public string Name { get; init; } = null!;
//        public string? Description { get; init; }
//        public string CurrencyId { get; init; } = null!;
//        public string SmtpHost { get; init; } = null!;
//        public int SmtpPort { get; init; }
//        public string SmtpUserName { get; init; } = null!;
//        public string SmtpPassword { get; init; } = null!;
//        public bool SmtpUseSSL { get; init; }
//        public bool Active { get; init; }
//    }

//    public class CreateConfigValidator : AbstractValidator<CreateConfigRequest>
//    {
//        public CreateConfigValidator()
//        {
//            RuleFor(x => x.Name)
//                .NotEmpty();

//            RuleFor(x => x.CurrencyId)
//                .NotEmpty();

//            RuleFor(x => x.SmtpHost)
//                .NotEmpty();

//            RuleFor(x => x.SmtpPort)
//                .GreaterThanOrEqualTo(0)
//                .WithMessage("SMTP Port must be greater than or equal to 0");

//            RuleFor(x => x.SmtpUserName)
//                .NotEmpty();

//            RuleFor(x => x.SmtpPassword)
//                .NotEmpty();
//        }
//    }


//    public class CreateConfigHandler : IRequestHandler<CreateConfigRequest, CreateConfigResult>
//    {
//        private readonly IBaseCommandRepository<Config> _repository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IEncryptionService _encryptionService;

//        public CreateConfigHandler(
//            IBaseCommandRepository<Config> repository,
//            IUnitOfWork unitOfWork,
//            IEncryptionService encryptionService
//            )
//        {
//            _repository = repository;
//            _unitOfWork = unitOfWork;
//            _encryptionService = encryptionService;
//        }

//        public async Task<CreateConfigResult> Handle(CreateConfigRequest request, CancellationToken cancellationToken = default)
//        {
//            //ensure have only one: active
//            if (request.Active == true)
//            {
//                var configs = _repository
//                    .GetQuery()
//                    .ApplyIsDeletedFilter()
//                    .ToList();

//                foreach (var item in configs)
//                {
//                    item.Active = false;
//                    item.Update(
//                        null,
//                        item.Title,
//                        item.Description,
//                        item.SmtpHost,
//                        item.SmtpPort,
//                        item.SmtpUserName,
//                        null,
//                        item.SmtpUseSSL,
//                        item.Active
//                    );
//                    _repository.Update(item);
//                }
//            }


//            var entity = new Config(
//                    request.UserId,
//                    request.Name,
//                    request.Description,
//                    request.SmtpHost,
//                    request.SmtpPort,
//                    request.SmtpUserName,
//                    _encryptionService.Encrypt(request.SmtpPassword),
//                    request.SmtpUseSSL,
//                    request.Active
//                    );

//            await _repository.CreateAsync(entity, cancellationToken);
//            await _unitOfWork.SaveAsync(cancellationToken);

//            return new CreateConfigResult
//            {
//                Id = entity.Id,
//                Message = "Success"
//            };
//        }
//    }
//}
