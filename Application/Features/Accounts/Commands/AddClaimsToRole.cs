using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands
{
    public class AddClaimsToRoleResult
    {
        public string? Role { get; init; }
        public string[] Claims { get; init; } = Array.Empty<string>();
    }

    public class AddClaimsToRoleRequest : IRequest<AddClaimsToRoleResult>
    {
        [Required]
        public string Role { get; init; }
        public string[] Claims { get; init; } = Array.Empty<string>();
    }

    public class AddClaimsToRoleValidator : AbstractValidator<AddClaimsToRoleRequest>
    {
        public AddClaimsToRoleValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty();

            RuleFor(x => x.Claims)
                .NotEmpty();
        }
    }

    public class AddClaimsToRoleHandler : IRequestHandler<AddClaimsToRoleRequest, AddClaimsToRoleResult>
    {
        private readonly IIdentityService _identityService;

        public AddClaimsToRoleHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<AddClaimsToRoleResult> Handle(AddClaimsToRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.AddClaimsToRoleAsync(
                request.Role,
                request.Claims,
                cancellationToken
                );

            return result;
        }
    }
}
