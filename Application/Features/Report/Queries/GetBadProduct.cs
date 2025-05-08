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
    public class BadProductDto
    {
        public string ProductName { get; set; }
        public int negativePercent { get; set; }
    }

    public class GetBadProductResult
    {
        public List<BadProductDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetBadProductRequest : IRequest<GetBadProductResult>
    {

    }

    public class GetBadProductHandler : IRequestHandler<GetBadProductRequest, GetBadProductResult>
    {
        private readonly IQueryContext _context;
        private readonly IGeneralReview _generalReview;

        public GetBadProductHandler(
            IQueryContext context,
            IGeneralReview generalReview
            )
        {
            _context = context;
            _generalReview = generalReview;
        }

        public async Task<GetBadProductResult> Handle(GetBadProductRequest request, CancellationToken cancellationToken)
        {
            var lstProduct = _context.Product
                                    .Select(x => new { x.Id, x.Title })
                                    .ToList();

            var resultList = new List<BadProductDto>();

            foreach (var product in lstProduct)
            {
                var reviews = await _context.ReviewProduct
                                            .Where(x => x.ProductId == product.Id)
                                            .ToListAsync(cancellationToken);

                List<int> predictions = new();
                if (reviews.Count > 0)
                {
                    var tasks = reviews.Select(r => _generalReview.GetGeneralReview(r.Content));
                    predictions = (await Task.WhenAll(tasks)).ToList();
                }

                var positiveCount = predictions.Count(x => x == 0);
                var negativeCount = predictions.Count(x => x == 1);
                var totalCount = positiveCount + negativeCount;
                int negativePercent = 0;
                if (totalCount > 0)
                {
                    negativePercent = (int)((double)negativeCount / totalCount * 100);
                }

                resultList.Add(new BadProductDto
                {
                    ProductName = product.Title,
                    negativePercent = negativePercent
                });
            }

            // Lấy top 5 sản phẩm có tỷ lệ đánh giá tiêu cực cao nhất
            var top5BadProducts = resultList
                .OrderByDescending(x => x.negativePercent)
                .ThenBy(x => x.ProductName)
                .Take(5)
                .ToList();

            return new GetBadProductResult
            {
                Data = top5BadProducts,
                Message = "Success"
            };
        }

    }
}
