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
 
    public class CategoryDto
    {
        public string Id { get; init; } = null!;
        public string Title { get; init; } = null!;
    }
    public class GetProductCategoryNameResult
    {
        public IEnumerable<UserDiscount> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductCategoryNameRequest : IRequest<GetProductCategoryNameResult>
    {
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
            var cateName = await _context.UserDiscount.ToListAsync( cancellationToken );

            return new GetProductCategoryNameResult
            {
                Data = cateName,
                Message = "Success"
            };
        }
    }
}
