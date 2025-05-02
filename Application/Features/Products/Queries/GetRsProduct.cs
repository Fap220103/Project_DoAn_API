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
    
    public class GetRsProductResult
    {
        public List<ProductDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }
    public class GetRsProductProfile : Profile
    {
        public GetRsProductProfile()
        {
            CreateMap<Product, ProductDto>()
                       .ForMember(dest => dest.ProductCategoryName,
                       opt => opt.MapFrom(src => src.ProductCategory.Title))
                          .ForMember(dest => dest.imageDefault,
                       opt => opt.MapFrom(src =>
                                src.ProductImage.FirstOrDefault(x => x.IsDefault && x.Image != null) != null
                                    ? src.ProductImage.FirstOrDefault(x => x.IsDefault && x.Image != null)!.Image
                                    : string.Empty
                            ));
            CreateMap<Product, PosProduceModel>()
                      .ForMember(dest => dest.Name,
                      opt => opt.MapFrom(src => src.Title))
                        .ForMember(dest => dest.Category,
                       opt => opt.MapFrom(src => src.ProductCategory.Title));
        }
    }
    public class GetRsProductRequest : IRequest<GetRsProductResult>
    {
        public string ProductId { get; set; } 

    }

    public class GetRsProductHandler : IRequestHandler<GetRsProductRequest, GetRsProductResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;
        private readonly IContentBaseFiltering _contentBaseFiltering;

        public GetRsProductHandler(
            IQueryContext context,
            IMapper mapper,
            IContentBaseFiltering contentBaseFiltering
            )
        {
            _context = context;
            _mapper = mapper;
            _contentBaseFiltering = contentBaseFiltering;
        }
        public async Task<GetRsProductResult> Handle(GetRsProductRequest request, CancellationToken cancellationToken)
        {
            var currentProduct = await _context.Product.Include(x=> x.ProductCategory)
                     .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken);
            var currentDto = _mapper.Map<PosProduceModel>(currentProduct);
            var recommendProducts = await _contentBaseFiltering.RecommendSimilarProductsAsync(new List<PosProduceModel> { currentDto }, 5);
            var recommendProductIds = recommendProducts.Select(p => p.Id).ToList();
            var findRsProduct = _context.Product
                .Where(x => recommendProductIds.Contains(x.Id))
                .Include(x => x.ProductImage)
                .Include(x => x.ProductCategory);

            var resultList = await findRsProduct.ToListAsync(cancellationToken);
            var mappedList = _mapper.Map<List<ProductDto>>(resultList);
         

            return new GetRsProductResult
            {
                Data = mappedList,
                Message = "Success"
            };
        }
    }
}
