using Application.Common.Models;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Features.Report.Queries
{
    public class RevenueDto
    {
        public DateTime Date { get; set; }
        public decimal DoanhThu { get; set; }
        public decimal LoiNhuan { get; set; }
    }

    public class GetRevenueResult
    {
        public List<RevenueDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetRevenueRequest : IRequest<GetRevenueResult>
    {
        public string? FromDate { get; set; } 
        public string? ToDate { get; set; }
    }

    public class GetRevenueHandler : IRequestHandler<GetRevenueRequest, GetRevenueResult>
    {
        private readonly IQueryContext _context;
        private readonly IIdentityService _identityService;

        public GetRevenueHandler(
            IQueryContext context,
            IIdentityService identityService

            )
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<GetRevenueResult> Handle(GetRevenueRequest request, CancellationToken cancellationToken)
        {
            var query = from o in _context.Order
                        join od in _context.OrderDetail on o.Id equals od.OrderId
                        join pv in _context.ProductVariant on od.ProductVariantId equals pv.Id
                        join p in _context.Product on pv.ProductId equals p.Id
                        select new
                        {
                            CreateDate = o.CreatedAt,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.OriginalPrice
                        };
            // Lọc theo FromDate và ToDate nếu có
            if (!string.IsNullOrEmpty(request.FromDate) && DateTime.TryParse(request.FromDate, out DateTime fromDate))
            {
                query = query.Where(x => x.CreateDate >= fromDate.Date);
            }

            if (!string.IsNullOrEmpty(request.ToDate) && DateTime.TryParse(request.ToDate, out DateTime toDate))
            {
                var endDate = toDate.Date.AddDays(1); // Bao gồm cả ngày kết thúc
                query = query.Where(x => x.CreateDate < endDate);
            }

            // Truy vấn và nhóm dữ liệu theo ngày
            var result = await query
                .GroupBy(x => x.CreateDate.HasValue ? x.CreateDate.Value.Date : DateTime.MinValue)
                .Select(x => new
                {
                    Date = x.Key,
                    TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                    TotalSell = x.Sum(y => y.Quantity * y.Price),
                })
                .Select(x => new RevenueDto
                {
                    Date = x.Date,
                    DoanhThu = x.TotalSell,
                    LoiNhuan = x.TotalSell - x.TotalBuy,
                })
                .ToListAsync(cancellationToken);


            return new GetRevenueResult
            {
                Data = result,
                Message = "Success"
            };
        }
    }
}
