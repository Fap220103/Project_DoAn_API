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
        public int avgRate { get; set; }
        public int totalRate { get; set; }
        public int totalSold { get; set; }
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

        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public string? Sizes { get; set; }
        public string? Colors { get; set; }
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
                                        .Include(x=> x.ReviewProducts)
                                        .AsQueryable();

            //lọc theo danh mục
            if (!string.IsNullOrEmpty(request.categoryId))
            {
                var categoryIds = request.categoryId.Split(',').ToList();
                query = query.Where(p => categoryIds.Contains(p.ProductCategoryId));
            }

            // tìm kiếm
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string searchKeyword = request.Search.Trim().ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(searchKeyword) ||
                    c.ProductCode.ToLower().Contains(searchKeyword) ||
                    c.Alias.ToLower().Contains(searchKeyword)
                );
            }

            // lọc theo giá
            if (request.PriceMin.HasValue)
            {
                query = query.Where(p => p.Price >= request.PriceMin.Value);
            }
            if (request.PriceMax.HasValue)
            {
                query = query.Where(p => p.Price <= request.PriceMax.Value);
            }

            //lọc theo màu
            if (!string.IsNullOrEmpty(request.Colors))
            {
                var colorNames = request.Colors.Split(',').Select(c => c.Trim()).ToList();
                var productVariantQuery = _context.ProductVariant.AsQueryable();
                var productIdsByColor = await _context.ProductVariant
                                                   .Where(pv => colorNames.Contains(pv.Color.Name))
                                                   .Select(pv => pv.ProductId)
                                                   .Distinct()
                                                   .ToListAsync(cancellationToken);

                query = query.Where(p => productIdsByColor.Contains(p.Id));
            }

            // Lọc theo size
            if (!string.IsNullOrEmpty(request.Sizes))
            {
                var sizeNames = request.Sizes.Split(',').Select(s => s.Trim()).ToList();

                var productIdsBySize = await _context.ProductVariant
                    .Where(pv => sizeNames.Contains(pv.Size.Name))
                    .Select(pv => pv.ProductId)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                query = query.Where(p => productIdsBySize.Contains(p.Id));
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
                        ("price", "asc") => query.OrderBy(x => x.PriceSale),
                        ("price", "desc") => query.OrderByDescending(x => x.PriceSale),
                        _ => query
                    };
                }
                else if (request.Order == "bestseller")
                {
                    query = query.OrderByDescending(p => _context.OrderDetail
                        .Where(od => od.ProductVariant.ProductId == p.Id)
                        .Sum(od => od.Quantity));
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            // phân trang
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            // mapping
            var dto = _mapper.Map<List<ProductClientDto>>(items).ToList();
            for (int i = 0; i < dto.Count; i++)
            {
                var product = items[i];
                // Tính trung bình đánh giá
                if (product.ReviewProducts != null && product.ReviewProducts.Any())
                {
                    dto[i].avgRate = (int)Math.Round(product.ReviewProducts.Average(r => r.Rate));
                    dto[i].totalRate = product.ReviewProducts.Count();
                }
                else
                {
                    dto[i].avgRate = 0;
                    dto[i].totalRate = 0;
                }

                // Tính tổng số đã bán từ OrderDetails nếu có
                dto[i].totalSold = _context.OrderDetail
                    .Where(od => od.ProductVariant.ProductId == product.Id)
                    .Sum(od => od.Quantity);
            }
            var pagedList = new PagedList<ProductClientDto>(dto, total, request.Page, request.Limit);
            return new GetProductByCategoryResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
