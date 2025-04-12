using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class UpdateProductResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateProductRequest : IRequest<UpdateProductResult>
    {
        public string UserId { get; init; } = null!;
        public string ProductId { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? Alias { get; set; }
        public string? Description { get; init; }
        public string? Image { get; set; }
        public string Detail { get; init; } = null!;
        public decimal OriginalPrice { get; init; }
        public decimal Price { get; init; }
        public int SalePercent { get; init; }
        public string? SeoDescription { get; init; }
        public string? SeoKeywords { get; init; }
        public string? SeoTitle { get; set; }
        public string? ProductCategoryId { get; init; }
        public bool IsActive { get; set; }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
            RuleFor(x => x.ProductId)
               .NotEmpty();
        }
    }


    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, UpdateProductResult>
    {
        private readonly IBaseCommandRepository<Product> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductHandler(
            IBaseCommandRepository<Product> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductResult> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ProductId}");
            }
            
            if(string.IsNullOrEmpty(request.Alias))
            {
                request.Alias = entity.Alias;
            }
            entity.Update(
                    request.UserId,
                    request.Title,
                    request.Alias,
                    request.Description,
                    request.SeoTitle,
                    request.SeoDescription,
                    request.SeoKeywords,
                    request.Detail,
                    request.OriginalPrice,
                    request.Price,
                    request.SalePercent,
                    request.IsActive,
                    request.ProductCategoryId
                );

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateProductResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
