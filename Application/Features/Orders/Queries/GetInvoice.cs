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
    public class InvoiceDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CustomerName { get; set; }
        public List<InvoiceItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal Total { get; set; }
        public ShippingAddress address { get; set; }
    }

    public class InvoiceItemDto
    {
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }

    public class GetInvoiceResult
    {
        public InvoiceDto Data { get; init; } = null!;

        public string Message { get; init; } = null!;
    }
    public class GetInvoiceProfile : Profile
    {
        public GetInvoiceProfile()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>();
        }
    }

    public class GetInvoiceRequest : IRequest<GetInvoiceResult>
    {
        public string OrderId { get; set; }
    }

    public class GetInvoiceHandler : IRequestHandler<GetInvoiceRequest, GetInvoiceResult>
    {
        private readonly IQueryContext _context;

        public GetInvoiceHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetInvoiceResult> Handle(GetInvoiceRequest request, CancellationToken cancellationToken)
        {
            var order = await _context.Order
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Product)
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Color)
                                        .Include(x => x.OrderDetails)
                                            .ThenInclude(od => od.ProductVariant)
                                                .ThenInclude(pv => pv.Size)
                                        .Include(x => x.ShippingAddress)
                                        .FirstOrDefaultAsync(o => o.Id == request.OrderId);

            if (order == null)
                throw new ApplicationException("Không tìm thấy orderid hợp lệ");

            var invoice = new InvoiceDto
            {
                InvoiceNumber = "HD-" + order.Id,
                CreatedDate = order.CreatedAt,
                CustomerName = order.ShippingAddress.RecipientName,
                Items = order.OrderDetails.Select(od => new InvoiceItemDto
                {
                    ProductName = od.ProductVariant.Product.Title,
                    ColorName = od.ProductVariant.Color.Name,
                    SizeName = od.ProductVariant.Size.Name,
                    Quantity = od.Quantity,
                    UnitPrice = od.Price
                }).ToList(),
                TotalAmount = order.TotalDiscount + order.TotalAmount,
                TotalDiscount = order.TotalDiscount,
                Total = order.TotalAmount,
                address = order.ShippingAddress
            };
            return new GetInvoiceResult
            {
                Data = invoice,
                Message = "Success"
            };
        }
    }
}
