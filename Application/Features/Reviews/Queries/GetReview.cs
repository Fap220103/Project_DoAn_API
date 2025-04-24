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

namespace Application.Features.Reviews.Queries
{
    public class ReviewDto
    {
        public string ProductId { get; init; } = null!;
        public string CustomerId { get; init; } = null!;
        public string CustomerName { get; init; } = null!;
        public string Content { get; init; } = null!;
        public int Rate { get; set; }
        public DateTime CreateDate { get; set; }
    }


    public class GetReviewProfile : Profile
    {
        public GetReviewProfile()
        {
            CreateMap<ReviewProduct, ReviewDto>();
        }
    }

    public class GetReviewResult
    {
        public PagedList<ReviewDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetReviewRequest : IRequest<GetReviewResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public int Rate { get; set; } = 5;
    }

    public class GetReviewHandler : IRequestHandler<GetReviewRequest, GetReviewResult>
    {
        private readonly IQueryContext _context;
        private readonly IIdentityService _identityService;

        public GetReviewHandler(
            IQueryContext context,
            IIdentityService identityService

            )
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<GetReviewResult> Handle(GetReviewRequest request, CancellationToken cancellationToken)
        {
            var query = _context.ReviewProduct.AsQueryable();

            if (request.Rate > 0)
            {
                query = query.Where(x => x.Rate == request.Rate);
            }

            // Sắp xếp
            query = query.OrderByDescending(x => x.CreatedDate);
            

            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await query
                .Skip(skip)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            // Mapping
            var dtoList = new List<ReviewDto>();

            foreach (var item in items)
            {
                var customerName = await _identityService.GetCustomerNameAsync(item.CustomerId);

                dtoList.Add(new ReviewDto
                {
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    CustomerName = customerName ?? "Unknown",
                    Content = item.Content,
                    Rate = item.Rate,
                    CreateDate = item.CreatedDate
                });
            }


            var total = await query.CountAsync(cancellationToken);
            var pagedList = new PagedList<ReviewDto>(dtoList, total, request.Page, request.Limit);

            return new GetReviewResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
