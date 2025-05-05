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
    public class BestSellingProductDto
    {
        public string ProductName { get; set; }
        public int QuantitySold { get; set; }
    }

    public class BestSellerResult
    {
        public List<BestSellingProductDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class BestSellerRequest : IRequest<BestSellerResult>
    {
        
    }

    public class BestSellerHandler : IRequestHandler<BestSellerRequest, BestSellerResult>
    {
        private readonly IQueryContext _context;

        public BestSellerHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<BestSellerResult> Handle(BestSellerRequest request, CancellationToken cancellationToken)
        {
            var result = _context.OrderDetail
            .Include(od => od.ProductVariant).ThenInclude(pv=> pv.Product)
            .GroupBy(od => od.ProductVariant.Product.Title)
            .Select(g => new BestSellingProductDto
            {
                ProductName = g.Key,
                QuantitySold = g.Sum(od => od.Quantity)
            })
            .OrderByDescending(x => x.QuantitySold)
            .Take(5)
            .ToList();

            return new BestSellerResult
            {
                Data = result,
                Message = "Success"
            };
        }
    }
}
