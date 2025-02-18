using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Commands
{
    public class CreateProductResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateProductRequest : IRequest<CreateProductResult>
    {
        public string? UserId { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string? Icon { get; init; }
        public string SeoTitle { get; init; } = null!;
        public string SeoDescription { get; init; } = null!;
        public string SeoKeywords { get; init; } = null!;
        public string ParentId { get; init; } = null!;
    }

    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();
        }
    }


    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResult>
    {
        private readonly IBaseCommandRepository<Product> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductHandler(
            IBaseCommandRepository<Product> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProductResult> Handle(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            ProductCategory? parentCategory = null;

            if (!string.IsNullOrWhiteSpace(request.ParentId))
            {
                parentCategory = await _repository.GetByIdAsync(request.ParentId, cancellationToken);
                if (parentCategory == null)
                {
                    throw new ApplicationException("Parent category not found.");
                }
            }
            var entity = new ProductCategory(
                    request.UserId,
                    request.Title,
                    request.Description,
                    request.SeoTitle,
                    request.SeoDescription,
                    request.SeoKeywords,
                    request.Icon,
                    request.ParentId
                    );
            if (parentCategory != null)
            {
                entity.ParentCategory = parentCategory;
                parentCategory.ChildCategories.Add(entity);
            }

            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateProductResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
