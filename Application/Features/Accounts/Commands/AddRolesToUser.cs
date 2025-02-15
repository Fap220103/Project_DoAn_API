using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands
{
    public class AddRolesToUserResult
    {
        public string? UserId { get; init; }
        public string[] Roles { get; init; } = Array.Empty<string>();
    }

    public class AddRolesToUserRequest : IRequest<AddRolesToUserResult>
    {
        
        public string UserId { get; init; }
        public string[] Roles { get; init; } = Array.Empty<string>();
    }

    public class AddRolesToUserValidator : AbstractValidator<AddRolesToUserRequest>
    {
        public AddRolesToUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Roles)
                .NotEmpty();
        }
    }

    public class AddRolesToUserHandler : IRequestHandler<AddRolesToUserRequest, AddRolesToUserResult>
    {
        private readonly IIdentityService _identityService;

        public AddRolesToUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AddRolesToUserResult> Handle(AddRolesToUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.AddRolesToUserAsync(
                request.UserId,
                request.Roles,
                cancellationToken
                );

            return result;
        }
    }
}
