using Application.Common.Models;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductVariants.Queries;
using Application.Services.CQS.Queries;
using Application.Services.Externals;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboard.Queries
{
    public class DashboardDto
    {
        public int totalOrder { get; set; }
        public int totalUser { get; set; }
        public int totalProduct { get; set; }
        public int totalInventory { get; set; }
    }
    public class GetInfoDashboardResult
    {
        public DashboardDto Data { get; init; } = null!;

        public string Message { get; init; } = null!;
    }

    public class GetInfoDashboardRequest : IRequest<GetInfoDashboardResult>
    {
    }

    public class GetInfoDashboardHandler : IRequestHandler<GetInfoDashboardRequest, GetInfoDashboardResult>
    {
        private readonly IQueryContext _context;
        private readonly IIdentityService _identityService;

        public GetInfoDashboardHandler(
            IQueryContext context,
            IIdentityService identityService
            )
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<GetInfoDashboardResult> Handle(GetInfoDashboardRequest request, CancellationToken cancellationToken)
        {
            var totalOrder = _context.Order.Count();
            var totalProduct = _context.Product.ApplyIsDeletedFilter().Count();
            var totalUser = await _identityService.CountUserAsync(cancellationToken);
            var totalInventory = _context.ProductVariant.Where(x=> x.Quantity < 5).Count();
            var result = new DashboardDto
            {
                totalOrder = totalOrder,
                totalProduct = totalProduct,
                totalUser = totalUser,
                totalInventory = totalInventory,
            };
            return new GetInfoDashboardResult
            {
                Data = result,
                Message = "Success"
            };
        }
    }
}
