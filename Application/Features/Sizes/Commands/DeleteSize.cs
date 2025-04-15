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

namespace Application.Features.Sizes.Commands
{
    public class DeleteSizeResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteSizeRequest : IRequest<DeleteSizeResult>
    {
        public string Id { get; init; } = null!;
    }

    public class DeleteSizeValidator : AbstractValidator<DeleteSizeRequest>
    {
        public DeleteSizeValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteSizeHandler : IRequestHandler<DeleteSizeRequest, DeleteSizeResult>
    {
        private readonly IBaseCommandRepository<Size> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSizeHandler(
            IBaseCommandRepository<Size> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteSizeResult> Handle(DeleteSizeRequest request, CancellationToken cancellationToken = default)
        {

            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            _repository.Purge(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteSizeResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
