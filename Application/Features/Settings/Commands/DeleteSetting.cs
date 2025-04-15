using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Configs.Commands
{
    public class DeleteSettingResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteSettingRequest : IRequest<DeleteSettingResult>
    {
        public string Id { get; init; } = null!;
        public string? UserId { get; init; }
    }

    public class DeleteSettingValidator : AbstractValidator<DeleteSettingRequest>
    {
        public DeleteSettingValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteSettingHandler : IRequestHandler<DeleteSettingRequest, DeleteSettingResult>
    {
        private readonly IBaseCommandRepository<Setting> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSettingHandler(
            IBaseCommandRepository<Setting> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteSettingResult> Handle(DeleteSettingRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            _repository.Delete(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteSettingResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }

}
