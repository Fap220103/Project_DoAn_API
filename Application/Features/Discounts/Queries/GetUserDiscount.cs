using Application.Common.Models;
using Application.Services.CQS.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Discounts.Queries
{
    public class DiscountDto
    {
        public string DiscountId { get; set; }
        public string Code { get; set; } = default!; 
        public string Title { get; set; } = default!; 
        public DiscountType DiscountType { get; set; } 
        public decimal DiscountValue { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountDto>()
                .ForMember(dest => dest.DiscountId, opt => opt.MapFrom(src => src.Id)); ;
        }
    }
    public class GetUserDiscountResult
    {
        public PagedList<DiscountDto> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetUserDiscountRequest : IRequest<GetUserDiscountResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string UserId { get; set; }

    }

    public class GetUserDiscountHandler : IRequestHandler<GetUserDiscountRequest, GetUserDiscountResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetUserDiscountHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetUserDiscountResult> Handle(GetUserDiscountRequest request, CancellationToken cancellationToken)
        {
            // Lấy danh sách DiscountId mà User đã lưu
            var userDiscountQuery = _context.UserDiscount
                .Where(x => x.UserId == request.UserId && x.IsUsed == false)
                .Select(x => x.DiscountId);

            // Lấy tất cả Discount tương ứng
            var discountQuery = _context.Discount
                .Where(d => userDiscountQuery.Contains(d.Id) && d.IsActive)
                .OrderByDescending(d => d.EndDate);

            // Phân trang
            var skip = (request.Page - 1) * request.Limit;
            var items = await discountQuery
                .Skip(skip)
                .Take(request.Limit)
                .ProjectTo<DiscountDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var total = await discountQuery.CountAsync(cancellationToken);
            var pagedList = new PagedList<DiscountDto>(items, total, request.Page, request.Limit);

            return new GetUserDiscountResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }

    }
}
