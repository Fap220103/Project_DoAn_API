using Application.Features.ProductCategories.Queries;
using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class ProductDto
    {
        public string Id { get; init; } = null!;
        public string? Alias { get; set; }
        public string ProductCode { get; init; } = null!;
        public string Detail { get; init; } = null!;
        public string? Image { get; set; }
        public decimal OriginalPrice { get; init; }
        public decimal Price { get; init; }
        public int SalePercent { get; init; }
        public decimal PriceSale { get; init; }
        public int ViewCount { get; set; }
        public bool IsSale { get; set; }
        public bool IsActive { get; set; }
        public bool isDeleted { get; set; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string ProductCategoryId { get; init; } = null!;
        public string SeoTitle { get; set; } = null!;
        public string SeoDescription { get; set; } = null!;
        public string SeoKeywords { get; set; } = null!;

    }


    public class GetAllProductProfile : Profile
    {
        public GetAllProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }

    public class GetAllProductResult
    {
        public IEnumerable<Product> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetAllProductRequest : IRequest<GetAllProductResult>
    {
    }

    public class GetAllProductValidator : AbstractValidator<GetAllProductRequest>
    {
        public GetAllProductValidator()
        {

        }
    }


    public class GetAllProductHandler : IRequestHandler<GetAllProductRequest, GetAllProductResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetAllProductHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetAllProductResult> Handle(GetAllProductRequest request, CancellationToken cancellationToken)
        {
            var entities = await _context.Product.ApplyIsDeletedFilter()
                                                .Include(x=> x.ProductImage)
                                                .ToListAsync(cancellationToken);

            var dto = _mapper.Map<IEnumerable<ProductDto>>(entities);

            return new GetAllProductResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
