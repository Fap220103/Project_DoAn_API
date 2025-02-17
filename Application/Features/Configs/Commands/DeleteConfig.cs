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
    public class DeleteConfigResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteConfigRequest : IRequest<DeleteConfigResult>
    {
        public string Id { get; init; } = null!;
        public string? UserId { get; init; }
    }

    public class DeleteConfigValidator : AbstractValidator<DeleteConfigRequest>
    {
        public DeleteConfigValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteConfigHandler : IRequestHandler<DeleteConfigRequest, DeleteConfigResult>
    {
        private readonly IBaseCommandRepository<Config> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteConfigHandler(
            IBaseCommandRepository<Config> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteConfigResult> Handle(DeleteConfigRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            entity.Delete(request.UserId);
            _repository.Delete(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteConfigResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }

}
