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

namespace Application.Features.ProductImages.Commands
{
    public class ChangeDefaultResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class ChangeDefaultRequest : IRequest<ChangeDefaultResult>
    {
        public string ImageId { get; init; } = null!;
        public string IdDefault { get; init; } = null!;
    }

    public class ChangeDefaultValidator : AbstractValidator<ChangeDefaultRequest>
    {
        public ChangeDefaultValidator()
        {
            RuleFor(x => x.ImageId)
                .NotEmpty();
            RuleFor(x => x.IdDefault)
                .NotEmpty();
        }
    }


    public class ChangeDefaultHandler : IRequestHandler<ChangeDefaultRequest, ChangeDefaultResult>
    {
        private readonly IBaseCommandRepository<ProductImage> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeDefaultHandler(
            IBaseCommandRepository<ProductImage> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChangeDefaultResult> Handle(ChangeDefaultRequest request, CancellationToken cancellationToken)
        {

            if (request.IdDefault != "-1")
            {
                var itemDefault = await _repository.GetByIdAsync(request.IdDefault);
                if (itemDefault != null)
                {
                    itemDefault.IsDefault = false;
                }
                else
                {
                    throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.IdDefault}");
                }
            }

            var item = await _repository.GetByIdAsync(request.ImageId);
            item.IsDefault = true;

            await _unitOfWork.SaveAsync(cancellationToken);

            return new ChangeDefaultResult
            {
                Id = item.Id,
                Message = "Success"
            };
        }
    }
}
