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

namespace Application.Features.Colors.Commands
{
    public class UpdateColorResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateColorRequest : IRequest<UpdateColorResult>
    {
        public string Id { get; init; } = null!;
        public string ColorName { get; init; } = null!;
        public string ColorCode { get; init; } = null!;

    }

    public class UpdateColorValidator : AbstractValidator<UpdateColorRequest>
    {
        public UpdateColorValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.ColorName)
                .NotEmpty();
            RuleFor(x => x.ColorCode)
              .NotEmpty();
        }
    }


    public class UpdateColorHandler : IRequestHandler<UpdateColorRequest, UpdateColorResult>
    {
        private readonly IBaseCommandRepository<Color> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateColorHandler(
            IBaseCommandRepository<Color> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateColorResult> Handle(UpdateColorRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            entity.Update(
                request.ColorName,
                request.ColorCode
                );

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateColorResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
