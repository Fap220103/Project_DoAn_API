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
    public class UpdateSizeResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateSizeRequest : IRequest<UpdateSizeResult>
    {
        public string Id { get; init; } = null!;
        public string SizeName { get; init; } = null!;
    }

    public class UpdateSizeValidator : AbstractValidator<UpdateSizeRequest>
    {
        public UpdateSizeValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.SizeName)
                .NotEmpty();
        }
    }


    public class UpdateSizeHandler : IRequestHandler<UpdateSizeRequest, UpdateSizeResult>
    {
        private readonly IBaseCommandRepository<Size> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSizeHandler(
            IBaseCommandRepository<Size> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateSizeResult> Handle(UpdateSizeRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            entity.Update(
                request.SizeName
                );

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateSizeResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
