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
    public class ReportDto
    {
        public string ReportTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalOrders { get; set; }
        public int TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public List<TopProduct> TopProducts { get; set; }
        public DateTime CreatedDate {  get; set; }
        public string CreateBy { get; set; }
    }
    public class TopProduct
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
    public class GetReportResult
    {
        public ReportDto Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetReportRequest : IRequest<GetReportResult>
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public string CreatedId { get; set; }
    }

    public class GetReportHandler : IRequestHandler<GetReportRequest, GetReportResult>
    {
        private readonly IQueryContext _context;
        private readonly IIdentityService _identityService;

        public GetReportHandler(
            IQueryContext context,
            IIdentityService identityService

            )
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<GetReportResult> Handle(GetReportRequest request, CancellationToken cancellationToken)
        {
            var fromDate = string.IsNullOrEmpty(request.FromDate)
                ? DateTime.Now.AddMonths(-1)
                : DateTime.Parse(request.FromDate);
            var toDate = string.IsNullOrEmpty(request.ToDate)
                ? DateTime.Now
                : DateTime.Parse(request.ToDate);

            var orders = await _context.Order
                   .Include(o => o.OrderDetails)
                      .ThenInclude(od => od.ProductVariant)
                         .ThenInclude(pv => pv.Product)
                   .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
                   .ToListAsync(cancellationToken);

            var totalOrders = orders.Count;
            var totalRevenue = orders.Sum(o => o.TotalAmount);
            var totalCustomers = orders.Select(o => o.CustomerId).Distinct().Count();

            var productStats = orders
                .SelectMany(o => o.OrderDetails)
               .GroupBy(d => d.ProductVariant.Product.Title)
                .Select(g => new TopProduct
                {
                    ProductName = g.Key,
                    Quantity = g.Sum(x => x.Quantity),
                    Revenue = g.Sum(x => x.Quantity * x.Price)
                })
                .OrderByDescending(p => p.Quantity)
                .Take(5)
                .ToList();

            // 5. Người tạo
            var createdBy = await _identityService.GetCustomerNameAsync(request.CreatedId);

            // 6. Kết quả
            var result = new ReportDto
            {
                ReportTitle = $"BÁO CÁO DOANH THU TỪ {fromDate:dd/MM/yyyy} - {toDate:dd/MM/yyyy}",
                FromDate = fromDate,
                ToDate = toDate,
                TotalOrders = totalOrders,
                TotalRevenue = (int)totalRevenue,
                TotalCustomers = totalCustomers,
                TopProducts = productStats,
                CreatedDate = DateTime.Now,
                CreateBy = createdBy
            };

            return new GetReportResult
            {
                Data = result,
                Message = "Success"
            };
        }
    }
}
