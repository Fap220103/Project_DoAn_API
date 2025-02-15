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
    public class GetRolesByUserResult
    {
        public PagedList<string> Roles { get; init; } = null!;
    }

    public class GetRolesByUserRequest : IRequest<GetRolesByUserResult>
    {
        public string UserId { get; init; }
        public int pageNumber { get; init; }
        public int pageSize { get; init; }
    }

    public class GetRolesByUserValidator : AbstractValidator<GetRolesByUserRequest>
    {
        public GetRolesByUserValidator()
        {
            RuleFor(x => x.pageNumber)
                .NotEmpty();

            RuleFor(x => x.pageSize)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }


    public class GetRolesByUserHandler : IRequestHandler<GetRolesByUserRequest, GetRolesByUserResult>
    {
        private readonly IIdentityService _identityService;

        public GetRolesByUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetRolesByUserResult> Handle(GetRolesByUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetRolesByUserAsync(
                request.UserId,
                request.pageNumber,
                request.pageSize,
                cancellationToken
                );

            return result;
        }
    }
}
