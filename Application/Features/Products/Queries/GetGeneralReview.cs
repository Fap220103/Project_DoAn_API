using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
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

namespace Application.Features.Products.Queries
{
    public class GeneralReviewDto
    {
        public int PositivePercent { get; set; }
        public int NegativePercent { get; set; }
        public string Suggest { get; set; }
    }


    public class GetGeneralReviewResult
    {
        public GeneralReviewDto Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetGeneralReviewRequest : IRequest<GetGeneralReviewResult>
    {
        public string productId { get; set; }
    }

    public class GetGeneralReviewHandler : IRequestHandler<GetGeneralReviewRequest, GetGeneralReviewResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;
        private readonly IGeneralReview _generalReviewService;

        public GetGeneralReviewHandler(
            IQueryContext context,
            IMapper mapper,
            IGeneralReview generalReviewService
            )
        {
            _context = context;
            _mapper = mapper;
            _generalReviewService = generalReviewService;
        }

        public async Task<GetGeneralReviewResult> Handle(GetGeneralReviewRequest request, CancellationToken cancellationToken)
        {
            var reviews = await _context.ReviewProduct.Where(x => x.ProductId == request.productId).ToListAsync(cancellationToken);
            List<int> predictions = new();
            if (reviews.Count > 0)
            {
                var tasks = reviews.Select(r => _generalReviewService.GetGeneralReview(r.Content));
                predictions = (await Task.WhenAll(tasks)).ToList();
            }

            var positiveCount = predictions.Count(x => x == 0);
            var negativeCount = predictions.Count(x => x == 1);
            var totalCount = positiveCount + negativeCount;

            int positivePercent = 0;
            int negativePercent = 0;
            string suggest = "Chưa đủ dữ liệu đánh giá";

            if (totalCount > 0)
            {
                positivePercent = (int)((double)positiveCount / totalCount * 100);
                negativePercent = (int)((double)negativeCount / totalCount * 100);

                if (positivePercent > 70)
                    suggest = "Nên mua";
                else if (positivePercent > 40)
                    suggest = "Cân nhắc";
                else
                    suggest = "Không nên mua";
            }

            var result = new GeneralReviewDto
            {
                PositivePercent = positivePercent,
                NegativePercent = negativePercent,
                Suggest = suggest
            };
            return new GetGeneralReviewResult
            {
                Data = result,
                Message = "Success"
            };
        }
    }
}
