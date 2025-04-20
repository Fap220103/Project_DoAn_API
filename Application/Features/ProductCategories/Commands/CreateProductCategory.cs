using Application.Services.Repositories;
using FluentValidation;
using MediatR;
using Domain.Entities;
using Application.Services.Externals;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductCategories.Commands
{
    public class CreateProductCategoryResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateProductCategoryRequest : IRequest<CreateProductCategoryResult>
    {
        public string Title { get; init; } = null!;
        public string? Alias { get; set; }
        public string? Description { get; init; }
        public int Level { get; init; } = 1;
        public string? ParentId { get; init; }
        public string? Link { get; set; }
    }

    public class CreateProductCategoryValidator : AbstractValidator<CreateProductCategoryRequest>
    {
        public CreateProductCategoryValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();
        }
    }


    public class CreateProductCategoryHandler : IRequestHandler<CreateProductCategoryRequest, CreateProductCategoryResult>
    {
        private readonly IBaseCommandRepository<ProductCategory> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommonService _commonService;

        public CreateProductCategoryHandler(
            IBaseCommandRepository<ProductCategory> repository,
            IUnitOfWork unitOfWork,
            ICommonService commonService
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
        }

        public async Task<CreateProductCategoryResult> Handle(CreateProductCategoryRequest request, CancellationToken cancellationToken = default)
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
            if (string.IsNullOrEmpty(request.Alias))
            {
                request.Alias = _commonService.FilterChar(request.Title);
            }
            var entity = new ProductCategory(
                    request.Title,
                    request.Alias,
                    request.Description,
                    request.Level,
                    request.ParentId,
                    request.Link
                    );
            if (parentCategory != null)
            {
                entity.ParentCategory = parentCategory;
                parentCategory.ChildCategories.Add(entity); 
            }

            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateProductCategoryResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
