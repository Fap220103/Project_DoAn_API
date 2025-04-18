using Application.Common.Models;
using Application.Features.Accounts.Dtos;
using Application.Features.ProductCategories.Queries;
using Application.Services.Externals;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Queries
{
    public class GetUsersResult
    {
        public PagedList<ApplicationUserDto>? Data { get; init; }
        public string Message { get; init; } = null!;
    }
    public class GetUsersRequest : IRequest<GetUsersResult>
    {
        public int Page { get; init; } = 1;
        public int Limit { get; init; } = 10;
        public string? Order { get; init; }
        public string? Search { get; init; }
        public string? Role { get; init; }
    }
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, GetUsersResult>
    {
        private readonly IIdentityService _identityService;

        public GetUsersHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetUsersResult> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetUsersAsync(
                request.Page,
                request.Limit,
                request.Order,
                request.Search,
                request.Role,
                cancellationToken);

            return new GetUsersResult
            {
                Data = result.Data,
                Message = "Success"
            };
        }
    }
}
