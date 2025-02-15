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
    public class GetClaimsResult
    {
        public PagedList<ClaimDto>? Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetClaimsRequest : IRequest<GetClaimsResult>
    {
        public int pageNumber { get; init; }
        public int pageSize { get; init; }
        public string SortBy { get; init; }
        public string SortDirection { get; init; }
        public string searchValue { get; init; } = string.Empty;
    }

    public class GetClaimsValidator : AbstractValidator<GetClaimsRequest>
    {
        public GetClaimsValidator()
        {
            RuleFor(x => x.pageNumber)
                .NotEmpty();

            RuleFor(x => x.pageSize)
                .NotEmpty();

            RuleFor(x => x.SortBy)
                .NotEmpty();

            RuleFor(x => x.SortDirection)
                .NotEmpty();
        }
    }


    public class GetClaimsHandler : IRequestHandler<GetClaimsRequest, GetClaimsResult>
    {
        private readonly IIdentityService _identityService;

        public GetClaimsHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetClaimsResult> Handle(GetClaimsRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetClaimsAsync(
                request.pageNumber,
                request.pageSize,
                request.SortBy,
                request.SortDirection,
                request.searchValue,
                cancellationToken
                );

            return new GetClaimsResult
            {
                Data = result.Data,
                Message = "Success"
            };
        }
    }
}
