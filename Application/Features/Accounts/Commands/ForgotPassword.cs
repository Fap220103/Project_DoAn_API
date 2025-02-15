using Application.Features.Accounts.Events;
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
    public class ForgotPasswordResult
    {
        public string Email { get; init; } = null!;
        public string TempPassword { get; init; } = null!;
        public string ClearTempPassword { get; init; } = null!;
        public string? EmailConfirmationToken { get; init; }
        public string Host { get; init; } = null!;
    }

    public class ForgotPasswordRequest : IRequest<ForgotPasswordResult>
    {
        public string Email { get; init; }
        public string Host { get; init; }
    }

    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Host)
                .NotEmpty();
        }
    }


    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordRequest, ForgotPasswordResult>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public ForgotPasswordHandler(
            IMediator mediator,
            IIdentityService identityService
            )
        {
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<ForgotPasswordResult> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ForgotPasswordAsync(
                request.Email,
                cancellationToken
                );

            var forgotPasswordEvent = new ForgotPasswordEvent
            (
                result.Email,
                result.TempPassword,
                result.EmailConfirmationToken,
                request.Host,
                result.ClearTempPassword
            );

            await _mediator.Publish(forgotPasswordEvent);

            return result;
        }
    }
}
