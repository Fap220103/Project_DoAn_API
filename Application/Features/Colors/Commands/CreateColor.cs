using Application.Services.Repositories;
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
    public class CreateColorResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateColorRequest : IRequest<CreateColorResult>
    {
        public string ColorName { get; init; } = null!;
        public string ColorCode { get; init; } = null!;
    }

    public class CreateColorValidator : AbstractValidator<CreateColorRequest>
    {
        public CreateColorValidator()
        {
            RuleFor(x => x.ColorName)
                .NotEmpty();
            RuleFor(x => x.ColorCode)
                .NotEmpty();
        }
    }


    public class CreateColorHandler : IRequestHandler<CreateColorRequest, CreateColorResult>
    {
        private readonly IBaseCommandRepository<Color> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateColorHandler(
            IBaseCommandRepository<Color> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateColorResult> Handle(CreateColorRequest request, CancellationToken cancellationToken = default)
        {
            var entity = new Color(
                    request.ColorName,
                    request.ColorCode
                    );

            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateColorResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
