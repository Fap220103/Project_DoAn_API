using Application.Services.Repositories;
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
    public class CreateSizeResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateSizeRequest : IRequest<CreateSizeResult>
    {
        public string SizeName { get; init; } = null!;
    }

    public class CreateSizeValidator : AbstractValidator<CreateSizeRequest>
    {
        public CreateSizeValidator()
        {
            RuleFor(x => x.SizeName)
                .NotEmpty();
        }
    }


    public class CreateSizeHandler : IRequestHandler<CreateSizeRequest, CreateSizeResult>
    {
        private readonly IBaseCommandRepository<Size> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSizeHandler(
            IBaseCommandRepository<Size> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSizeResult> Handle(CreateSizeRequest request, CancellationToken cancellationToken = default)
        {
            var entity = new Size(
                    request.SizeName
                    );

            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateSizeResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
