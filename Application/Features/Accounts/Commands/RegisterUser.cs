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
    public class RegisterUserResult
    {
        public string? Id { get; init; }
        public string Email { get; init; } = null!;
        public string? UserName { get; init; }
        public string? EmailConfirmationToken { get; init; }
        public bool SendEmailConfirmation { get; init; }
    }

    public class RegisterUserRequest : IRequest<RegisterUserResult>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
    }

    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password).WithMessage("Password and Confirm Password should equal.");
        }
    }


    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResult>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public RegisterUserHandler(
            IMediator mediator,
            IIdentityService identityService
            )
        {
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RegisterUserAsync(
                request.Email,
                request.Password,
                cancellationToken
                );

            var registerUserEvent = new RegisterUserEvent
            (
                result.Email,
                result.EmailConfirmationToken,
                result.SendEmailConfirmation
            );
            await _mediator.Publish(registerUserEvent);

            return result;
        }
    }

}
