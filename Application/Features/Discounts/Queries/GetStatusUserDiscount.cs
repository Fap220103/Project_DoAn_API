using Application.Services.CQS.Queries;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Discounts.Queries
{
    public class GetStatusUserDiscountResult
    {
        public bool Data { get; init; }

        public string Message { get; init; } = null!;
    }

    public class GetStatusUserDiscountRequest : IRequest<GetStatusUserDiscountResult>
    {
        public string UserId { get; set; }
        public string DiscountId { get; set; }
    }
    public class GetStatusUserDiscountValidator : AbstractValidator<GetStatusUserDiscountRequest>
    {
        public GetStatusUserDiscountValidator()
        {
            RuleFor(x => x.DiscountId)
                .NotEmpty();
            RuleFor(x => x.UserId)
              .NotEmpty();
        }
    }

    public class GetStatusUserDiscountHandler : IRequestHandler<GetStatusUserDiscountRequest, GetStatusUserDiscountResult>
    {
        private readonly IQueryContext _context;

        public GetStatusUserDiscountHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetStatusUserDiscountResult> Handle(GetStatusUserDiscountRequest request, CancellationToken cancellationToken)
        {
            var hasSavedDiscount = await _context.UserDiscount
                                       .AnyAsync(ud => ud.UserId == request.UserId && ud.DiscountId == request.DiscountId);



            return new GetStatusUserDiscountResult
            {
                Data = hasSavedDiscount,
                Message = "Success"
            };
        }
    }
}
