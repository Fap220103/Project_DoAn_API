using Application.Services.Repositories;
using FluentValidation;
using MediatR;
using Domain.Entities;
using Application.Services.Externals;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Application.Services.CQS.Queries;

namespace Application.Features.ProductVariants.Commands
{
    public class CreateProductVariantResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateProductVariantRequest : IRequest<CreateProductVariantResult>
    {
        public string ProductId { get; init; }
        public List<int> ColorId { get; set; }
        public List<int> SizeId { get; init; }
    }

    public class CreateProductVariantValidator : AbstractValidator<CreateProductVariantRequest>
    {
        public CreateProductVariantValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
            RuleFor(x => x.ColorId)
               .NotEmpty();
            RuleFor(x => x.SizeId)
               .NotEmpty();
        }
    }


    public class CreateProductVariantHandler : IRequestHandler<CreateProductVariantRequest, CreateProductVariantResult>
    {
        private readonly IBaseCommandRepository<ProductVariant> _repository;
        private readonly IQueryContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductVariantHandler(
            IBaseCommandRepository<ProductVariant> repository,
            IQueryContext context,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProductVariantResult> Handle(CreateProductVariantRequest request, CancellationToken cancellationToken = default)
        {
            var productVariants = new List<ProductVariant>();

            var existingVariants = await _context.ProductVariant
                .Where(x => x.ProductId == request.ProductId
                         && request.ColorId.Contains(x.ColorId)
                         && request.SizeId.Contains(x.SizeId))
                .ToListAsync(cancellationToken);

            bool IsDuplicate(int colorId, int sizeId) =>
                existingVariants.Any(x => x.ColorId == colorId && x.SizeId == sizeId);

            foreach (var colorId in request.ColorId)
            {
                foreach (var sizeId in request.SizeId)
                {
                    if (IsDuplicate(colorId, sizeId)) continue;

                    var variant = new ProductVariant
                    {
                        Id = Guid.NewGuid().ToString(),
                        ProductId = request.ProductId,
                        ColorId = colorId,
                        SizeId = sizeId
                    };

                    productVariants.Add(variant);
                }
            }

            if (productVariants.Any())
            {
                await _repository.AddRangeAsync(productVariants, cancellationToken);
                await _unitOfWork.SaveAsync();
            }

            return new CreateProductVariantResult
            {
                Id = request.ProductId,
                Message = "Success"
            };
        }
    }
}
