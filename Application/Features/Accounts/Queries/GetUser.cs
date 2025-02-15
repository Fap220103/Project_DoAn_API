using Application.Common.Models;
using Application.Features.Accounts.Dtos;
using Application.Services.Externals;
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
        public int Page { get; init; } 
        public int Limit { get; init; }
        public string SortBy { get; init; }
        public string SortDirection { get; init; }
        public string searchValue { get; init; } = string.Empty;
    }
    public class GetUsersValidator : AbstractValidator<GetUsersRequest>
    {
        public GetUsersValidator()
        {
            RuleFor(x => x.Page)
                .NotEmpty();

            RuleFor(x => x.Limit)
                .NotEmpty();

            RuleFor(x => x.SortBy)
                .NotEmpty();

            RuleFor(x => x.SortDirection)
                .NotEmpty();
        }
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
                request.SortBy,
                request.SortDirection,
                request.searchValue,
                cancellationToken);

            return new GetUsersResult
            {
                Data = result.Data,
                Message = "Success"
            };
        }
    }
}
