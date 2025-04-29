using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductVariants.Queries;
using Application.Features.Reviews.Commands;
using Application.Services.CQS.Queries;
using Application.Services.Externals;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries
{
   
    public class GetStatusUserOrderResult
    {
        public bool Data { get; init; } 

        public string Message { get; init; } = null!;
    }

    public class GetStatusUserOrderRequest : IRequest<GetStatusUserOrderResult>
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
    }
    public class GetStatusUserOrderValidator : AbstractValidator<GetStatusUserOrderRequest>
    {
        public GetStatusUserOrderValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
            RuleFor(x => x.UserId)
              .NotEmpty();
        }
    }

    public class GetStatusUserOrderHandler : IRequestHandler<GetStatusUserOrderRequest, GetStatusUserOrderResult>
    {
        private readonly IQueryContext _context;

        public GetStatusUserOrderHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetStatusUserOrderResult> Handle(GetStatusUserOrderRequest request, CancellationToken cancellationToken)
        {
            var hasOrdered = await _context.Order.Where(x=> x.CustomerId == request.UserId && x.Status == 3)
                                                    .AnyAsync(x=> x.OrderDetails
                                                    .Any(od => od.ProductVariant.ProductId == request.ProductId), cancellationToken);

   
            return new GetStatusUserOrderResult
            {
                Data = hasOrdered,
                Message = "Success"
            };
        }
    }
}
