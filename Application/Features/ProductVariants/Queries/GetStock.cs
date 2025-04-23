using Application.Common.Models;
using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductVariants.Queries
{
    public class GetStockResult
    {
        public int Data { get; init; }
        public string Message { get; init; } = null!;
    }

    public class GetStockRequest : IRequest<GetStockResult>
    {
        public string ProductVariantId { get; set; }
    }

    public class GetStockHandler : IRequestHandler<GetStockRequest, GetStockResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;
        public GetStockHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetStockResult> Handle(GetStockRequest request, CancellationToken cancellationToken)
        {
            var variant = _context.ProductVariant.FirstOrDefault(v => v.Id == request.ProductVariantId);
            if (variant == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound}");
            }
            return new GetStockResult
            {
                Data = variant.Quantity,
                Message = "Success"
            };
        }
    }
}
