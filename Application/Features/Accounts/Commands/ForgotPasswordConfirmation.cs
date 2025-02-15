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
    public class ForgotPasswordConfirmationResult
    {
        public string? Email { get; init; }
    }

    public class ForgotPasswordConfirmationRequest : IRequest<ForgotPasswordConfirmationResult>
    {
        public string Email { get; init; }
        public string TempPassword { get; init; }
        public string Code { get; init; }
    }

    public class ForgotPasswordConfirmationValidator : AbstractValidator<ForgotPasswordConfirmationRequest>
    {
        public ForgotPasswordConfirmationValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.TempPassword)
                .NotEmpty();

            RuleFor(x => x.Code)
                .NotEmpty();
        }
    }


    public class ForgotPasswordConfirmationHandler : IRequestHandler<ForgotPasswordConfirmationRequest, ForgotPasswordConfirmationResult>
    {
        private readonly IIdentityService _identityService;

        public ForgotPasswordConfirmationHandler(
            IIdentityService identityService
            )
        {
            _identityService = identityService;
        }

        public async Task<ForgotPasswordConfirmationResult> Handle(ForgotPasswordConfirmationRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ForgotPasswordConfirmationAsync(
                request.Email,
                request.TempPassword,
                request.Code,
                cancellationToken
                );

            return new ForgotPasswordConfirmationResult { Email = result };
        }
    }
}
