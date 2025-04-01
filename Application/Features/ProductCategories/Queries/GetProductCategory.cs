using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Queries
{
    public class ProductCategoryDto
    {
        public string Id { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string? Alias { get; set; }
        public string? Icon { get; set; }
        public string? ParentName { get; set; }
        public bool IsActive { get; set; }
        public string SeoTitle { get; set; } = null!;
        public string SeoDescription { get; set; } = null!;
        public string SeoKeywords { get; set; } = null!;
        public ICollection<ProductCategoryDto> ChildCategories { get; set; } = new List<ProductCategoryDto>();
    }


    public class GetProductCategoryProfile : Profile
    {
        public GetProductCategoryProfile()
        {
            CreateMap<ProductCategory, ProductCategoryDto>();
        }
    }

    public class GetProductCategoryResult
    {
        public IEnumerable<ProductCategoryDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductCategoryRequest : IRequest<GetProductCategoryResult>
    {
    }

    public class GetProductCategoryValidator : AbstractValidator<GetProductCategoryRequest>
    {
        public GetProductCategoryValidator()
        {

        }
    }


    public class GetProductCategoryHandler : IRequestHandler<GetProductCategoryRequest, GetProductCategoryResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetProductCategoryHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetProductCategoryResult> Handle(GetProductCategoryRequest request, CancellationToken cancellationToken)
        {
            //var allCategories = await _context.ProductCategory.ApplyIsDeletedFilter().ToListAsync(cancellationToken);

            //var lookup = allCategories.ToLookup(c => c.ParentId);

            //foreach (var category in allCategories)
            //{
            //    category.ChildCategories = lookup[category.Id].ToList();
            //}

            //var rootCategories = allCategories.Where(c => c.ParentId == null).ToList();

            //var dto = _mapper.Map<IEnumerable<ProductCategoryDto>>(rootCategories);


            //return new GetProductCategoryResult
            //{
            //    Data = dto,
            //    Message = "Success"
            //};
            var allCategories = await _context.ProductCategory.ApplyIsDeletedFilter().ToListAsync(cancellationToken);

            // Tạo lookup để ánh xạ Id với danh mục
            var categoryDict = allCategories.ToDictionary(c => c.Id, c => c);

            // Gán ChildCategories
            var lookup = allCategories.ToLookup(c => c.ParentId);
            foreach (var category in allCategories)
            {
                category.ChildCategories = lookup[category.Id].ToList();
            }

            // Lấy danh mục gốc
            var rootCategories = allCategories.Where(c => c.ParentId == null).ToList();

            // Chuyển đổi sang DTO và gán ParentName
            var dto = _mapper.Map<IEnumerable<ProductCategoryDto>>(rootCategories);
            foreach (var categoryDto in dto)
            {
                SetParentName(categoryDto, categoryDict);
            }

            return new GetProductCategoryResult
            {
                Data = dto,
                Message = "Success"
            };
        }
        private void SetParentName(ProductCategoryDto dto, Dictionary<string, ProductCategory> categoryDict)
        {
            if (dto.Id != null && categoryDict.ContainsKey(dto.Id))
            {
                var parentId = categoryDict[dto.Id].ParentId;
                if (parentId != null && categoryDict.ContainsKey(parentId))
                {
                    dto.ParentName = categoryDict[parentId].Title;
                }
            }

            foreach (var child in dto.ChildCategories)
            {
                SetParentName(child, categoryDict);
            }
        }
    }
}
