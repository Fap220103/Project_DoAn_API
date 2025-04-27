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
    public class GetAddressDefaultResult
    {
        public ShippingAddress Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetAddressDefaultRequest : IRequest<GetAddressDefaultResult>
    {
        public string userId { get; set; }
    }

    public class GetAddressDefaultHandler : IRequestHandler<GetAddressDefaultRequest, GetAddressDefaultResult>
    {
        private readonly IQueryContext _context;

        public GetAddressDefaultHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetAddressDefaultResult> Handle(GetAddressDefaultRequest request, CancellationToken cancellationToken)
        {
            var query = _context.ShippingAddress.FirstOrDefault(x => x.UserId == request.userId && x.IsDefault);
           
            return new GetAddressDefaultResult
            {
                Data = query,
                Message = "Success"
            };
        }
    }
}
