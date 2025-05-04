using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
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

namespace Application.Features.Products.Queries
{
    public class ProductSuggestDto
    {
        public string Id { get; init; } = null!;  
        public string Title { get; init; } = null!;
        public string? imageDefault { get; set; } = string.Empty;
    }

    public class GetSuggestProductProfile : Profile
    {
        public GetSuggestProductProfile()
        {
            CreateMap<Product, ProductSuggestDto>()
                      .ForMember(dest => dest.imageDefault,
                       opt => opt.MapFrom(src =>
                                src.ProductImage.FirstOrDefault(x => x.IsDefault && x.Image != null) != null
                                    ? src.ProductImage.FirstOrDefault(x => x.IsDefault && x.Image != null)!.Image
                                    : string.Empty
                            ));
        }
    }

    public class GetSuggestProductResult
    {
        public PagedList<ProductSuggestDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetSuggestProductRequest : IRequest<GetSuggestProductResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? keyword { get; set; }
    }

    public class GetSuggestProductHandler : IRequestHandler<GetSuggestProductRequest, GetSuggestProductResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetSuggestProductHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetSuggestProductResult> Handle(GetSuggestProductRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Product.ApplyIsDeletedFilter()
                                        .Include(x => x.ProductImage)
                                        .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.keyword))
            {
                string searchKeyword = request.keyword.Trim().ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(searchKeyword) ||
                    c.Alias.ToLower().Contains(searchKeyword)
                );
            }
            query = query.OrderBy(x => x.Title);
            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var dto = _mapper.Map<IEnumerable<ProductSuggestDto>>(items).ToList();

            var pagedList = new PagedList<ProductSuggestDto>(dto, total, request.Page, request.Limit);
            return new GetSuggestProductResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
