//using Application.Services.CQS.Queries;
//using Application.Services.Externals;
//using Application.Services.Repositories;
//using Domain.Constants;
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
//    public class UpdateConfigResult
//    {
//        public string Id { get; init; } = null!;
//        public string Message { get; init; } = null!;
//    }

//    public class UpdateConfigRequest : IRequest<UpdateConfigResult>
//    {
//        public string Id { get; init; } = null!;
//        public string? UserId { get; init; }
//        public string Name { get; init; } = null!;
//        public string? Description { get; init; }
//        public string CurrencyId { get; init; } = null!;
//        public string SmtpHost { get; init; } = null!;
//        public int SmtpPort { get; init; }
//        public string SmtpUserName { get; init; } = null!;
//        public string? SmtpPassword { get; init; }
//        public bool SmtpUseSSL { get; init; }
//        public bool Active { get; init; }
//    }

//    public class UpdateConfigValidator : AbstractValidator<UpdateConfigRequest>
//    {
//        public UpdateConfigValidator()
//        {
//            RuleFor(x => x.Id)
//                .NotEmpty();

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
//        }
//    }


//    public class UpdateConfigHandler : IRequestHandler<UpdateConfigRequest, UpdateConfigResult>
//    {
//        private readonly IBaseCommandRepository<Config> _repository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IEncryptionService _encryptionService;

//        public UpdateConfigHandler(
//            IBaseCommandRepository<Config> repository,
//            IUnitOfWork unitOfWork,
//            IEncryptionService encryptionService
//            )
//        {
//            _repository = repository;
//            _unitOfWork = unitOfWork;
//            _encryptionService = encryptionService;
//        }

//        public async Task<UpdateConfigResult> Handle(UpdateConfigRequest request, CancellationToken cancellationToken = default)
//        {

//            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

//            if (entity == null)
//            {
//                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
//            }

//            //ensure have only one: active
//            if (request.Active == true)
//            {
//                var configs = _repository
//                    .GetQuery()
//                    .Where(x => x.Id != entity.Id)
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

//            entity.Update(
//                request.UserId,
//                request.Name,
//                request.Description,
//                request.SmtpHost,
//                request.SmtpPort,
//                request.SmtpUserName,
//                !string.IsNullOrEmpty(request.SmtpPassword) ? _encryptionService.Encrypt(request.SmtpPassword) : "",
//                request.SmtpUseSSL,
//                request.Active
//                );

//            _repository.Update(entity);
//            await _unitOfWork.SaveAsync(cancellationToken);

//            return new UpdateConfigResult
//            {
//                Id = entity.Id,
//                Message = "Success"
//            };
//        }
//    }
//}
