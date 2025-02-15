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
    public class UpdateRoleResult
    {
        public string? Role { get; init; }
        public string[]? Claims { get; init; }
    }

    public class UpdateRoleRequest : IRequest<UpdateRoleResult>
    {
        public string OldRole { get; init; }
        public string NewRole { get; init; }
        public string[] Claims { get; init; }
    }

    public class UpdateRoleValidator : AbstractValidator<UpdateRoleRequest>
    {
        public UpdateRoleValidator()
        {
            RuleFor(x => x.OldRole)
                .NotEmpty();

            RuleFor(x => x.NewRole)
                .NotEmpty();

            RuleFor(x => x.Claims)
                .NotNull();
        }
    }


    public class UpdateRoleHandler : IRequestHandler<UpdateRoleRequest, UpdateRoleResult>
    {
        private readonly IIdentityService _identityService;

        public UpdateRoleHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UpdateRoleResult> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateRoleAsync(
                request.OldRole,
                request.NewRole,
                request.Claims,
                cancellationToken
                );

            return result;
        }
    }
}
