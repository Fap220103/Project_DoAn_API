using Application.Services.Repositories;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Discounts.Commands
{
    public class AddUserDiscountResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddUserDiscountRequest : IRequest<AddUserDiscountResult>
    {
        public string UserId { get; init; } = null!;
        public string DiscountId { get; init; } = null!;
       
    }

    public class AddUserDiscountValidator : AbstractValidator<AddUserDiscountRequest>
    {
        public AddUserDiscountValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.DiscountId)
                .NotEmpty();
        }
    }


    public class AddUserDiscountHandler : IRequestHandler<AddUserDiscountRequest, AddUserDiscountResult>
    {
        private readonly IBaseCommandRepository<UserDiscount> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public AddUserDiscountHandler(
            IBaseCommandRepository<UserDiscount> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddUserDiscountResult> Handle(AddUserDiscountRequest request, CancellationToken cancellationToken = default)
        {
            var existing = await _repository
                .FindAsync(x => x.UserId == request.UserId && x.DiscountId == request.DiscountId, cancellationToken);

            if (existing != null)
            {
                throw new ApplicationException("Mã giảm giá này đã được lưu rồi");
            }
            var entity = new UserDiscount
            {
                UserId = request.UserId,
                DiscountId = request.DiscountId,
                IsUsed = false,
            };

            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new AddUserDiscountResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
