using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
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
     //public string? Alias { get; set; }
     //   public string? ProductCode { get; set; }
     //   [AllowHtml]
     //   public string? Detail { get; set; }
     //   public string? Image { get; set; }
     //   public decimal? OriginalPrice { get; set; }
     //   public decimal Price { get; set; }
     //   public decimal? PriceSale { get; set; }
     //   public int? ViewCount { get; set; }
     //   public bool IsSale { get; set; }
     //   public bool IsActive { get; set; }
     //   public string? ProductCategoryId { get; set; }
     //   public ProductCategory ProductCategory { get; set; }
     //   public ICollection<ProductImage> ProductImage { get; set; } = new Collection<ProductImage>();
     //   public ICollection<OrderDetail> OrderDetails { get; set; } = new Collection<OrderDetail>();
     //   public ICollection<ProductColor> ProductColor { get; set; } = new Collection<ProductColor>();
     //   public ICollection<ProductSize> ProductSize { get; set; } = new Collection<ProductSize>();
     //   public ICollection<Inventory> Inventory { get; set; } = new Collection<Inventory>();
    public class CreateProductRequest : IRequest<CreateProductResult>
    {
        public string? UserId { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
        public string? Image { get; init; }
        public string Detail { get; init; } = null!;
        public decimal OriginalPrice { get; init; } 
        public decimal Price { get; init; }
        public int SalePercent { get; init; }
        public string SeoDescription { get; init; } = null!;
        public string SeoKeywords { get; init; } = null!;
        public string SeoTitle { get; init; } = null!;
        public string? ProductCategoryId { get; init; }
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

            var entity = new Product(
                    request.UserId,
                    request.Title,
                    request.Description,
                    request.SeoTitle,
                    request.SeoDescription,
                    request.SeoKeywords,
                    request.Image,
                    request.Detail,
                    request.OriginalPrice,
                    request.Price,
                    request.SalePercent,
                    request.ProductCategoryId
                    );
         
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
