using Application.Services.CQS.Queries;
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

namespace Application.Features.ProductCategories.Queries
{

    public class GetProductCategoryNameResult
    {
        public string Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductCategoryNameRequest : IRequest<GetProductCategoryNameResult>
    {
        public string parentId { get; init; } = null!;
    }
    public class GetProductCategoryNameHandler : IRequestHandler<GetProductCategoryNameRequest, GetProductCategoryNameResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetProductCategoryNameHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetProductCategoryNameResult> Handle(GetProductCategoryNameRequest request, CancellationToken cancellationToken)
        {
            var cateName = await _context.ProductCategory
                                  .Where(x => x.Id == request.parentId)
                                  .Select(x => x.Title)
                                  .FirstOrDefaultAsync(cancellationToken) ?? "";

            return new GetProductCategoryNameResult
            {
                Data = cateName,
                Message = "Success"
            };
        }
    }
}
