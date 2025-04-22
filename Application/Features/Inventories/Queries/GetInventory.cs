using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductVariants.Queries;
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

namespace Application.Features.Inventories.Queries
{
    public class InventoryDto
    {
        public string ProductVariantId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductCode { get; set; } = null!;
        public string ColorName { get; set; } = null!;
        public string SizeName { get; set; } = null!;
        public int Quantity { get; set; }
    }
    public class ProductVariantProfile : Profile
    {
        public ProductVariantProfile()
        {
            CreateMap<Inventory, InventoryDto>()
              .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductVariant.Product.Title))
                 .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProductVariant.Product.ProductCode))
              .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ProductVariant.Color.Name))
              .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductVariant.Size.Name))
              .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
    public class GetInventoryResult
    {
        public PagedList<InventoryDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetInventoryRequest : IRequest<GetInventoryResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Search { get; set; }
    }

    public class GetInventoryHandler : IRequestHandler<GetInventoryRequest, GetInventoryResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;
        public GetInventoryHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetInventoryResult> Handle(GetInventoryRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Inventory
            .Include(i => i.ProductVariant)
                .ThenInclude(pv => pv.Product)
            .Include(i => i.ProductVariant.Color)
            .Include(i => i.ProductVariant.Size)
            .AsQueryable()
            .AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
            {
                var keyword = request.Search.ToLower();

                query = query.Where(x =>
                    x.ProductVariant.Product.Title.ToLower().Contains(keyword) ||
                    x.ProductVariant.Color.Name.ToLower().Contains(keyword) ||
                    x.ProductVariant.Size.Name.ToLower().Contains(keyword)
                );
            }

            query = query.OrderByDescending(x => x.ProductVariant.Product.Title);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dto = _mapper.Map<List<InventoryDto>>(items);
            var pagedList = new PagedList<InventoryDto>(dto, total, request.Page, request.Limit);

            return new GetInventoryResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
