using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
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
    public class GetProductByNameDto
    {
        public string Id { get; init; } = null!;
        public string? Alias { get; set; }
        public string ProductCode { get; init; } = null!;
        public string Detail { get; init; } = null!;
        public string? Image { get; set; }
        public decimal OriginalPrice { get; init; }
        public decimal Price { get; init; }
        public int SalePercent { get; init; }
        public decimal PriceSale { get; init; }
        public int ViewCount { get; set; }
        public bool IsSale { get; set; }
        public bool IsActive { get; set; }
        public bool isDeleted { get; set; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string ProductCategoryId { get; init; } = null!;
        public string SeoTitle { get; set; } = null!;
        public string SeoDescription { get; set; } = null!;
        public string SeoKeywords { get; set; } = null!;
    }


    public class GetProductByNameProfile : Profile
    {
        public GetProductByNameProfile()
        {
            //CreateMap<Product, GetProductByNameDto>();
        }
    }

    public class GetProductByNameResult
    {
        public List<Product> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductByNameRequest : IRequest<GetProductByNameResult>
    {
        public string ProductName { get; init; } = null!;
        public string Type { get; init; } = null!;
    }

    public class GetProductByNameValidator : AbstractValidator<GetProductByNameRequest>
    {
        public GetProductByNameValidator()
        {

        }
    }


    public class GetProductByNameHandler : IRequestHandler<GetProductByNameRequest, GetProductByNameResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetProductByNameHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetProductByNameResult> Handle(GetProductByNameRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Product
                                .Include(x => x.ProductImage)
                                .AsQueryable();

            if (request.Type.ToUpper() == "Equal")
            {
                query = query.Where(x => x.Title.Equals(request.ProductName));
            }
            else if (request.Type.ToUpper() == "In")
            {
                var productNames = request.ProductName.Split(',').Select(p => p.Trim()).ToList();
                query = query.Where(x => productNames.Contains(x.Title));
            }
            else
            {
                query = query.Where(x => x.Title.Contains(request.ProductName));
            }

            var result = await query.ToListAsync(cancellationToken);
            if (!result.Any())
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ProductName}");
            }
            return new GetProductByNameResult
            {
                Data = result,
                Message = "Success"
            };
        }

    }
}
