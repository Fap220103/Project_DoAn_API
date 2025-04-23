using Application.Services.Externals;
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
    public class CreateDiscountResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateDiscountRequest : IRequest<CreateDiscountResult>
    {
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? Description { get; init; }

        public DiscountType DiscountType { get; init; }
        public decimal DiscountValue { get; init; }

        public DateTime EndDate { get; init; }
        public int UsageLimit { get; init; } 
    }

    public class CreateDiscountValidator : AbstractValidator<CreateDiscountRequest>
    {
        public CreateDiscountValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã khuyến mãi không được để trống.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề không được để trống.");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("Giá trị khuyến mãi phải lớn hơn 0.");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Ngày kết thúc phải sau thời điểm hiện tại.");
        }
    }


    public class CreateDiscountHandler : IRequestHandler<CreateDiscountRequest, CreateDiscountResult>
    {
        private readonly IBaseCommandRepository<Discount> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateDiscountHandler(
            IBaseCommandRepository<Discount> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateDiscountResult> Handle(CreateDiscountRequest request, CancellationToken cancellationToken = default)
        {
            var entity = new Discount(
              request.Code,
              request.Title,
              request.Description,
              request.DiscountType,
              request.DiscountValue,
              request.EndDate,
              request.UsageLimit
          );

            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateDiscountResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
