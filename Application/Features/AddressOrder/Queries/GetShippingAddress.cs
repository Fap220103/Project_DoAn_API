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
        public IEnumerable<ShippingAddress> Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetShippingAddressRequest : IRequest<GetShippingAddressResult>
    {
        public string userId { get; set; }
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
            var entities = await _context.ShippingAddress.Where(x=> x.UserId == request.userId).ToListAsync(cancellationToken);

            return new GetShippingAddressResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
