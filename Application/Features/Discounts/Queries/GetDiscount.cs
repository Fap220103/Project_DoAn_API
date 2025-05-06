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

namespace Application.Features.Discounts.Queries
{
    public class GetDiscountResult
    {
        public PagedList<Discount> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetDiscountRequest : IRequest<GetDiscountResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; }
        public string? Search { get; set; }
    }

    public class GetDiscountHandler : IRequestHandler<GetDiscountRequest, GetDiscountResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetDiscountHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetDiscountResult> Handle(GetDiscountRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Discount.AsQueryable();

            var now = DateTime.UtcNow;

            query = query
                    .Where(d =>
                        d.IsActive &&
                        d.EndDate > now &&
                        (!d.UsageLimit.HasValue || d.UsedCount < d.UsageLimit.Value)
                    );

            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await query
                .Skip(skip)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

           
            var total = await query.CountAsync(cancellationToken);
            var pagedList = new PagedList<Discount>(items, total, request.Page, request.Limit);

            return new GetDiscountResult
            {
                Data = pagedList,
                Message = "Success"
            };

        }

    }
}
