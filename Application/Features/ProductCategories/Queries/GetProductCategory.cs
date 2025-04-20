using Application.Common.Models;
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
using System.Reflection.Emit;
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
        public string? ParentId { get; set; }
        public string? ParentName { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public string Link { get; set; } = null!;
        public bool IsHasChild { get; set; } 
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
        public PagedList<ProductCategoryDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetProductCategoryRequest : IRequest<GetProductCategoryResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; }
        public string? Search { get; set; }
        public int Level { get; set; } = 1;
        public string? ParentId { get; set; }
        public bool IsHasChild { get; set; } = false;
        public string? Filter { get; set; }
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
            var query = _context.ProductCategory
                                .Include(pc => pc.ParentCategory)
                                .AsQueryable();
            // Search theo parentId
            if (!string.IsNullOrEmpty(request.ParentId))
            {
                query = query.Where(x => x.ParentId == request.ParentId);
            }

            // Search theo Title hoặc Alias
            if (!string.IsNullOrEmpty(request.Search))
            {
                var keyword = request.Search.ToLower();
                query = query.Where(x =>
                    x.Title.ToLower().Contains(keyword) ||
                    (x.Alias != null && x.Alias.ToLower().Contains(keyword))
                );
            }

            if(!request.IsHasChild && request.Level == 2 && !string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(x => x.IsHasChild == false);
            }

        
            query = query.Where(x => x.Level == request.Level);
            

            // Sắp xếp
            if (!string.IsNullOrEmpty(request.Order))
            {
                
            }
            else
            {
                query = query.OrderByDescending(x => x.Title); // mặc định
            }

            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await query
                .Skip(skip)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            // Mapping
            var dto = _mapper.Map<List<ProductCategoryDto>>(items);

            // Gán thêm tên danh mục cha (nếu có)
            foreach (var item in dto)
            {
                var parent = items.FirstOrDefault(x => x.Id == item.Id)?.ParentCategory;
                item.ParentName = parent?.Title;
            }
            var total = await query.CountAsync(cancellationToken);
            var pagedList = new PagedList<ProductCategoryDto>(dto, total, request.Page, request.Limit);

            return new GetProductCategoryResult
            {
                Data = pagedList,
                Message = "Success"
            };

        }
     
    }
}
