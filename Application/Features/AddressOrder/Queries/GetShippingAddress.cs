using Application.Common.Models;
using Application.Services.CQS.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AddressOrder.Queries
{
    public class GetShippingAddressResult
    {
        public PagedList<ShippingAddress> Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetShippingAddressRequest : IRequest<GetShippingAddressResult>
    {
        public string userId { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }

    public class GetShippingAddressHandler : IRequestHandler<GetShippingAddressRequest, GetShippingAddressResult>
    {
        private readonly IQueryContext _context;

        public GetShippingAddressHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetShippingAddressResult> Handle(GetShippingAddressRequest request, CancellationToken cancellationToken)
        {
            var query = _context.ShippingAddress.Where(x=> x.UserId == request.userId).AsQueryable();
            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await query
                .Skip(skip)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);


            var total = await query.CountAsync(cancellationToken);
            var pagedList = new PagedList<ShippingAddress>(items, total, request.Page, request.Limit);
            return new GetShippingAddressResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
