using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductVariants.Queries;
using Application.Services.CQS.Queries;
using Application.Services.Externals;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries
{
    public class OrderDto
    {
        public string OrderId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public int TotalQuantity { get; set; }
        public string OrderCode { get; set; } = null!;
        public int TypePayment { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderDetailDto> items { get; set; } = new();
        public ShippingAddressDto address { get; set; } = new();
    }
    public class OrderDetailDto
    {
        public string ProductVariantId { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
    }
    public class ShippingAddressDto
    {
        public string UserId { get; set; }
        public string RecipientName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
    }

    public class GetOrderProfile : Profile
    {
        public GetOrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.OrderCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.address, opt => opt.MapFrom(src => src.ShippingAddress)); ;

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductVariantId, opt => opt.MapFrom(src => src.ProductVariantId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductVariant.Product.Title))  
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.ProductVariant.Color.Name))         
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.ProductVariant.Size.Name));        

            CreateMap<ShippingAddress, ShippingAddressDto>();

        }
    }

    public class GetOrderResult
    {
        public PagedList<OrderDto> Data { get; init; } = null!;

        public string Message { get; init; } = null!;
    }

    public class GetOrderRequest : IRequest<GetOrderResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? userId { get; set; }
        public int? Status { get; set; } 
        public string? Search { get; set; }
    }

    public class GetOrderHandler : IRequestHandler<GetOrderRequest, GetOrderResult>
    {
        private readonly IQueryContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public GetOrderHandler(
            IQueryContext context,
            IIdentityService identityService,
            IMapper mapper

            )
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
        }

        public async Task<GetOrderResult> Handle(GetOrderRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Order.Include(x => x.OrderDetails)
                                        .ThenInclude(od => od.ProductVariant)
                                            .ThenInclude(pv => pv.Product) 
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Color) 
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Size) 
                                        .Include(x => x.ShippingAddress)
                                        .AsQueryable();

            if (!string.IsNullOrEmpty(request.userId))
            {
                query = query.Where(x=> x.CustomerId == request.userId);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string searchKeyword = request.Search.Trim().ToLower();
                query = query.Where(c =>
                    c.ShippingAddress.RecipientName.ToLower().Contains(searchKeyword) ||
                    c.Code.ToLower().Contains(searchKeyword)
                );
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }


            query = query.OrderByDescending(x=> x.CreatedAt);

            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await query
                .Skip(skip)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var total = await query.CountAsync(cancellationToken);
            var dto = _mapper.Map<List<OrderDto>>(items);
            var pagedList = new PagedList<OrderDto>(dto, total, request.Page, request.Limit);
            return new GetOrderResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
