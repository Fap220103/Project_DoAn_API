using Application.Services.CQS.Queries;
using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
        public string? Alias { get; set; }
        public string? Description { get; init; }
        public string? ProductCode { get; set; } = null!;
        public string? Image { get; set; }
        public string Detail { get; init; } = null!;
        public decimal OriginalPrice { get; init; } 
        public decimal Price { get; init; }
        public int SalePercent { get; init; }
        public string? SeoDescription { get; init; }
        public string? SeoKeywords { get; init; }
        public string? SeoTitle { get; set; } 
        public string? ProductCategoryId { get; init; }
        public List<IFormFile>? Images { get; init; }
        public List<int>? Default { get; init; }
    }

    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Title)
           .NotEmpty().WithMessage("Title không được để trống.");

            RuleFor(x => x.OriginalPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("OriginalPrice phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Price phải lớn hơn hoặc bằng 0.");

            RuleFor(x => x.SalePercent)
                .InclusiveBetween(0, 100)
                .WithMessage("SalePercent phải nằm trong khoảng từ 0 đến 100.");
        }
    }


    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResult>
    {
        private readonly IBaseCommandRepository<Product> _repoProduct;
        private readonly IBaseCommandRepository<ProductImage> _repoProductImage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        private readonly ICommonService _commonService;
        private readonly ILogger<CreateProductHandler> _logger;
        private readonly IQueryContext _context;

        public CreateProductHandler(
            IBaseCommandRepository<Product> repoProduct,
            IBaseCommandRepository<ProductImage> repoProductImage,
            IUnitOfWork unitOfWork,
            IPhotoService photoService,
            ICommonService commonService,
            ILogger<CreateProductHandler> logger,
            IQueryContext context
            )
        {
            _repoProduct = repoProduct;
            _repoProductImage = repoProductImage;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _commonService = commonService;
            _logger = logger;
            _context = context;
        }

        public async Task<CreateProductResult> Handle(CreateProductRequest request, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(request.SeoTitle))
            {
                request.SeoTitle = request.Title;
            }
            if (string.IsNullOrEmpty(request.Alias))
            {
                request.Alias = _commonService.FilterChar(request.Title);
            }
            
            var entity = new Product(
                    request.UserId,
                    request.Title,
                    request.Alias,
                    request.Description,
                    request.SeoTitle,
                    request.SeoDescription,
                    request.SeoKeywords,
                    null,
                    request.Detail,
                    request.OriginalPrice,
                    request.Price,
                    request.SalePercent,
                    request.ProductCategoryId
                    );
            //if (string.IsNullOrEmpty(request.ProductCode)) 
            //{
            //    entity.ProductCode = _commonService.GenerateCode("PRO");
            //}
            //else
            //{
            //    entity.ProductCode = request.ProductCode;
            //}
            if (!string.IsNullOrEmpty(request.ProductCode))
            {
                var existed = await _context.Product.FirstOrDefaultAsync(x => x.ProductCode == request.ProductCode, cancellationToken);
                if (existed != null)
                {
                    throw new ApplicationException($"Mã sản phẩm '{request.ProductCode}' đã tồn tại.");
                }
                entity.ProductCode = request.ProductCode;
            }
            else
            {
                entity.ProductCode = _commonService.GenerateCode("PRO");
            }
            await _repoProduct.CreateAsync(entity, cancellationToken);

            if (request.Images != null && request.Images.Any())
            {
                int defaultImageIndex = request.Default?.ToList().FindIndex(x => x == 1) + 1 ?? 1;

                for (int i = 0; i < request.Images.Count; i++)
                {
                    var imageFile = request.Images[i];
                    var uploadResult = await _photoService.AddPhotoAsync(imageFile);
                    if (uploadResult == null)
                    {
                        _logger.LogError("Lỗi upload ảnh tại ảnh thứ {Index}", i + 1);
                        continue; 
                    }

                    bool isDefault = (i + 1 == defaultImageIndex);
                    if (isDefault)
                    {
                        entity.Image = uploadResult.Url.ToString();
                    }
                    var productImage = new ProductImage
                    {
                        ProductId = entity.Id,
                        Image = uploadResult.Url.ToString(),
                        IsDefault = isDefault
                    };
                    await _repoProductImage.CreateAsync(productImage, cancellationToken);
                }
            }
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateProductResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
