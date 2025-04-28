using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Commands
{
    public class UpdateDiscountResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateDiscountRequest : IRequest<UpdateDiscountResult>
    {
        public string Id { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public DiscountType DiscountType { get; init; }
        public decimal DiscountValue { get; init; }
        public DateTime EndDate { get; init; }
        public int UsageLimit { get; init; }
        public bool IsActive { get; init; }
    }

    public class UpdateDiscountValidator : AbstractValidator<UpdateDiscountRequest>
    {
        public UpdateDiscountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Code)
                .NotEmpty();
        }
    }


    public class UpdateDiscountHandler : IRequestHandler<UpdateDiscountRequest, UpdateDiscountResult>
    {
        private readonly IBaseCommandRepository<Discount> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateDiscountHandler(
            IBaseCommandRepository<Discount> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateDiscountResult> Handle(UpdateDiscountRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            entity.Update(
                request.Code,
                request.Title,
                request.Description,
                request.DiscountType,
                request.DiscountValue,
                request.EndDate,
                request.UsageLimit,
                request.IsActive
            );

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateDiscountResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
