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
    public class CreateRoleResult
    {
        public string? Role { get; init; }
        public string[]? Claims { get; init; }
    }

    public class CreateRoleRequest : IRequest<CreateRoleResult>
    {
        public string Role { get; init; }
        public string[] Claims { get; init; }
    }

    public class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty();

            RuleFor(x => x.Claims)
                .NotNull();
        }
    }


    public class CreateRoleHandler : IRequestHandler<CreateRoleRequest, CreateRoleResult>
    {
        private readonly IIdentityService _identityService;

        public CreateRoleHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<CreateRoleResult> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateRoleAsync(
                request.Role,
                request.Claims,
                cancellationToken
                );

            return result;
        }
    }
}
