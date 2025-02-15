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
    public class GetClaimsByUserResult
    {
        public PagedList<ClaimDto> Claims { get; init; } = null!;
    }

    public class GetClaimsByUserRequest : IRequest<GetClaimsByUserResult>
    {
        public string UserId { get; init; }
        public int pageNumber { get; init; }
        public int pageSize { get; init; }
        public string SortBy { get; init; }
        public string SortDirection { get; init; }
        public string searchValue { get; init; } = string.Empty;
    }

    public class GetClaimsByUserValidator : AbstractValidator<GetClaimsByUserRequest>
    {
        public GetClaimsByUserValidator()
        {
            RuleFor(x => x.pageNumber)
                .NotEmpty();

            RuleFor(x => x.pageSize)
                .NotEmpty();

            RuleFor(x => x.SortBy)
                .NotEmpty();

            RuleFor(x => x.SortDirection)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }


    public class GetClaimsByUserHandler : IRequestHandler<GetClaimsByUserRequest, GetClaimsByUserResult>
    {
        private readonly IIdentityService _identityService;

        public GetClaimsByUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetClaimsByUserResult> Handle(GetClaimsByUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetClaimsByUserAsync(
                request.UserId,
                request.pageNumber,
                request.pageSize,
                request.SortBy,
                request.SortDirection,
                request.searchValue,
                cancellationToken
                );

            return result;
        }
    }
}
