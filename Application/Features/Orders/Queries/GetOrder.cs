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
        public List<OrderDetailDto> items { get; set; } = new();
    }
    public class OrderDetailDto
    {
        public string ProductVariantId { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
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
                 .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.OrderDetails));

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductVariantId, opt => opt.MapFrom(src => src.ProductVariantId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
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
            var query = _context.Order.Include(x=> x.OrderDetails).AsQueryable();
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
