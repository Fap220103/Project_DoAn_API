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
   
    public class GetOrderByIdProfile : Profile
    {
        public GetOrderByIdProfile()
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

    public class GetOrderByIdResult
    {
        public OrderDto Data { get; init; } = null!;

        public string Message { get; init; } = null!;
    }

    public class GetOrderByIdRequest : IRequest<GetOrderByIdResult>
    {
        public string orderId { get; set; } 
    }

    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(
            IQueryContext context,
            IMapper mapper

            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetOrderByIdResult> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Order.Include(x => x.OrderDetails)
                                        .ThenInclude(od => od.ProductVariant)
                                            .ThenInclude(pv => pv.Product) // Bao gồm thông tin sản phẩm
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Color)  // Bao gồm thông tin màu sắc
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Size)   // Bao gồm thông tin kích thước
                                        .Include(x => x.ShippingAddress)
                                        .FirstOrDefault(x=> x.Id == request.orderId || x.Code == request.orderId);

         
            var dto = _mapper.Map<OrderDto>(query);
            return new GetOrderByIdResult
            {
                Data = dto,
                Message = "Success"
            };
        }
    }
}
