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
        public string? UserId { get; init; }
        public string Title { get; init; } = null!;
        public string? Alias { get; set; }
        public string? Description { get; init; }
        public int level { get; init; }
        public string SeoTitle { get; init; } = null!;
        public string SeoDescription { get; init; } = null!;
        public string SeoKeywords { get; init; } = null!;
        public string? ParentId { get; init; }
        public IFormFile Image { get; init; }
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
        private readonly IPhotoService _photoService;

        public CreateProductCategoryHandler(
            IBaseCommandRepository<ProductCategory> repository,
            IUnitOfWork unitOfWork,
            ICommonService commonService,
            IPhotoService photoService
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _commonService = commonService;
            _photoService = photoService;
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
                    request.UserId,
                    request.Title,
                    request.Alias,
                    request.Description,
                    request.SeoTitle,
                    request.SeoDescription,
                    request.SeoKeywords,
                    null,
                    request.level,
                    request.ParentId
                    );
            if (parentCategory != null)
            {
                entity.ParentCategory = parentCategory;
                parentCategory.ChildCategories.Add(entity); 
            }

            await _repository.CreateAsync(entity, cancellationToken);
            var uploadResult = await _photoService.AddPhotoAsync(request.Image);
            if (uploadResult == null)
            {
                throw new ApplicationException("Lỗi upload ảnh");
            }
                entity.Icon = uploadResult.Url.ToString();
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateProductCategoryResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
