using Application.Common.Models;
using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public class ProductClientDto
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
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string ProductCategoryId { get; init; } = null!;
        public string? ProductCategoryName { get; set; }
        public string imageDefault { get; set; } = null!;
    }

    public class GetProductByCategoryProfile : Profile
    {
        public GetProductByCategoryProfile()
        {
            CreateMap<Product, ProductClientDto>()
                       .ForMember(dest => dest.ProductCategoryName,
                       opt => opt.MapFrom(src => src.ProductCategory.Title))
                          .ForMember(dest => dest.imageDefault,
                       opt => opt.MapFrom(src => src.ProductImage.FirstOrDefault(x => x.IsDefault).Image ?? "default.jpg"));
        }
    }

    public class GetProductByCategoryResult
    {
        public PagedList<ProductClientDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductByCategoryRequest : IRequest<GetProductByCategoryResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; }
        public string? Search { get; set; }
        public string? categoryId { get; set; }
    }

    public class GetProductByCategoryHandler : IRequestHandler<GetProductByCategoryRequest, GetProductByCategoryResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetProductByCategoryHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Product.ApplyIsDeletedFilter()
                                        .Include(x => x.ProductImage)
                                        .Include(x => x.ProductCategory)
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(request.categoryId))
            {
                var categoryIds = request.categoryId.Split(',').ToList();
                query = query.Where(p => categoryIds.Contains(p.ProductCategoryId));
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string searchKeyword = request.Search.Trim().ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(searchKeyword) ||
                    c.Alias.ToLower().Contains(searchKeyword)
                );
            }
         
          
            // Sắp xếp nếu có order
            if (!string.IsNullOrEmpty(request.Order))
            {
                var parts = request.Order.Split('|');
                if (parts.Length == 2)
                {
                    var field = parts[0].ToLower();
                    var direction = parts[1].ToLower();

                    query = (field, direction) switch
                    {
                        ("title", "asc") => query.OrderBy(x => x.Title),
                        ("title", "desc") => query.OrderByDescending(x => x.Title),
                        ("viewcount", "asc") => query.OrderBy(x => x.ViewCount),
                        ("viewcount", "desc") => query.OrderByDescending(x => x.ViewCount),
                        _ => query
                    };
                }
            }
            else
            {
                query = query.OrderBy(x => x.Title);
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);
            var dto = _mapper.Map<IEnumerable<ProductClientDto>>(items).ToList();

            var pagedList = new PagedList<ProductClientDto>(dto, total, request.Page, request.Limit);
            return new GetProductByCategoryResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
