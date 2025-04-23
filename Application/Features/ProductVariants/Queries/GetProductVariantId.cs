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
    public class GetProductVariantIdResult
    {
        public string Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductVariantIdRequest : IRequest<GetProductVariantIdResult>
    {

        public string ProductId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
    }

    public class GetProductVariantIdHandler : IRequestHandler<GetProductVariantIdRequest, GetProductVariantIdResult>
    {
        private readonly IQueryContext _context;

        public GetProductVariantIdHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetProductVariantIdResult> Handle(GetProductVariantIdRequest request, CancellationToken cancellationToken)
        {
            var variant = _context.ProductVariant
                                  .FirstOrDefault(v => v.ProductId == request.ProductId &&
                                                  v.ColorId == request.ColorId &&
                                                  v.SizeId == request.SizeId);

            if (variant == null)
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound}");

            return new GetProductVariantIdResult
            {
                Data = variant.Id,
                Message = "Success"
            };

        }

    }
}
