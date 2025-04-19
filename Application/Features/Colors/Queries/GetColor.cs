using Application.Common.Models;
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

namespace Application.Features.Colors.Queries
{
   
    public class GetColorResult
    {
        public PagedList<Color> Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetColorRequest : IRequest<GetColorResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; }
        public string? Search { get; set; }
    }

    public class GetColorHandler : IRequestHandler<GetColorRequest, GetColorResult>
    {
        private readonly IQueryContext _context;

        public GetColorHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetColorResult> Handle(GetColorRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Color.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                string searchKeyword = request.Search.Trim().ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchKeyword) ||  // Tìm kiếm trong tên màu
                    c.HexCode.ToLower().Contains(searchKeyword)     // Tìm kiếm trong mã màu
                );
            }

            // Sắp xếp nếu có order
            if (!string.IsNullOrEmpty(request.Order))
            {
                var parts = request.Order.Split('|');
                if (parts.Length == 2)
                {
                    var field = parts[0].ToLower();
                    var direction = parts[1].ToLower();

                    query = (field, direction) switch
                    {
                        ("name", "asc") => query.OrderBy(x => x.Name),
                        ("name", "desc") => query.OrderByDescending(x => x.Name),
                        ("hexCode", "asc") => query.OrderBy(x => x.HexCode),
                        ("hexCode", "desc") => query.OrderByDescending(x => x.HexCode),
                        _ => query
                    };
                }
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var pagedList = new PagedList<Color>(items, total, request.Page, request.Limit);

            return new GetColorResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
