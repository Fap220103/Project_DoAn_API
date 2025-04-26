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
    public class ResetPasswordResult
    {
        public string? Email { get; init; }
    }

    public class ResetPasswordRequest : IRequest<ResetPasswordResult>
    {
        public string Email { get; init; }
        public string NewPassword { get; init; }
        public string Code { get; init; }
    }

    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPassword)
                .NotEmpty();

            RuleFor(x => x.Code)
                .NotEmpty();
        }
    }


    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequest, ResetPasswordResult>
    {
        private readonly IIdentityService _identityService;

        public ResetPasswordHandler(
            IIdentityService identityService
            )
        {
            _identityService = identityService;
        }

        public async Task<ResetPasswordResult> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ResetPasswordAsync(
                request.Email,
                request.NewPassword,
                request.Code,
                cancellationToken
                );

            return new ResetPasswordResult { Email = result };
        }
    }
}
