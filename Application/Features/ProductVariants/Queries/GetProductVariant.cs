using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
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

namespace Application.Features.ProductVariants.Queries
{

    public class ProductVariantDto
    {
        public string Id { get; init; } = null!;
        public string ProductTitle { get; set; }
        public string ProductCode { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
    }
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<ProductVariant, ProductVariantDto>()
                .ForMember(dest => dest.ProductTitle, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Product.ProductCode))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.Name));
        }
    }
    public class GetProductVariantResult
    {
        public PagedList<ProductVariantDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductVariantRequest : IRequest<GetProductVariantResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Search { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
    }

    public class GetProductVariantHandler : IRequestHandler<GetProductVariantRequest, GetProductVariantResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetProductVariantHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetProductVariantResult> Handle(GetProductVariantRequest request, CancellationToken cancellationToken)
        {
            var query = _context.ProductVariant
                                .Include(p => p.Product)
                                .Include(p => p.Color)
                                .Include(p => p.Size)
                                .AsQueryable()
                                .AsNoTracking();
            // Lọc theo ColorId nếu có
            if (request.ColorId.HasValue)
            {
                query = query.Where(x => x.ColorId == request.ColorId.Value);
            }

            // Lọc theo SizeId nếu có
            if (request.SizeId.HasValue)
            {
                query = query.Where(x => x.SizeId == request.SizeId.Value);
            }

            // Search theo Title hoặc Alias
            if (!string.IsNullOrEmpty(request.Search))
            {
                var keyword = request.Search.ToLower();
                query = query.Where(x =>
                    x.Product.Title.ToLower().Contains(keyword) ||
                     x.Product.ProductCode.ToLower().Contains(keyword) ||
                    x.Product.Id == keyword
                );
            }

            query = query.OrderByDescending(x => x.Product.Title); // mặc định
            
            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await query
                .Skip(skip)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            // Mapping
            var dto = _mapper.Map<List<ProductVariantDto>>(items);
          
            var total = await query.CountAsync(cancellationToken);
            var pagedList = new PagedList<ProductVariantDto>(dto, total, request.Page, request.Limit);

            return new GetProductVariantResult
            {
                Data = pagedList,
                Message = "Success"
            };

        }

    }
}
