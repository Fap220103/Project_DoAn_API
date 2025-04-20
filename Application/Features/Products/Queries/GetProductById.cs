using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Constants;
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
    public class GetProductByIdDto
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
        public string? ProductCategoryName { get; set; }
        public string SeoTitle { get; set; } = null!;
        public string SeoDescription { get; set; } = null!;
        public string SeoKeywords { get; set; } = null!;
    }


    public class GetProductByIdProfile : Profile
    {
        public GetProductByIdProfile()
        {
            CreateMap<Product, GetProductByIdDto>()
                .ForMember(dest => dest.ProductCategoryName,
                       opt => opt.MapFrom(src => src.ProductCategory.Title)); ;
        }
    }

    public class GetProductByIdResult
    {
        public GetProductByIdDto Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductByIdRequest : IRequest<GetProductByIdResult>
    {
        public string ProductId { get; init; } = null!; 
    }

    public class GetProductByIdValidator : AbstractValidator<GetProductByIdRequest>
    {
        public GetProductByIdValidator()
        {

        }
    }


    public class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetProductByIdResult> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var entity = await _context.Product.ApplyIsDeletedFilter()
                                                .Include(x=> x.ProductImage)
                                                .Include(x => x.ProductCategory)
                                                .Where(x=> x.Id == request.ProductId)
                                                .SingleOrDefaultAsync(cancellationToken);
            if(entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ProductId}");
            }
            var dto = _mapper.Map<GetProductByIdDto>(entity);
            return new GetProductByIdResult
            {
                Data = dto,
                Message = "Success"
            };
        }
    }
}
