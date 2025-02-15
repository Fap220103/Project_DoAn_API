using Application.Common.Models;
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
    public class GetClaimsByRoleResult
    {
        public PagedList<string> Claims { get; init; } = null!;
    }

    public class GetClaimsByRoleRequest : IRequest<GetClaimsByRoleResult>
    {
        public string Role { get; init; }
        public int pageNumber { get; init; }
        public int pageSize { get; init; }
    }

    public class GetClaimsByRoleValidator : AbstractValidator<GetClaimsByRoleRequest>
    {
        public GetClaimsByRoleValidator()
        {
            RuleFor(x => x.pageNumber)
                .NotEmpty();

            RuleFor(x => x.pageSize)
                .NotEmpty();

            RuleFor(x => x.Role)
                .NotEmpty();
        }
    }


    public class GetClaimsByRoleHandler : IRequestHandler<GetClaimsByRoleRequest, GetClaimsByRoleResult>
    {
        private readonly IIdentityService _identityService;

        public GetClaimsByRoleHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetClaimsByRoleResult> Handle(GetClaimsByRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetClaimsByRoleAsync(
                request.Role,
                request.pageNumber,
                request.pageSize,
                cancellationToken
                );

            return result;
        }
    }
}
