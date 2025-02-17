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
    public class CreateUserResult
    {
        public string? Id { get; init; }
        public string? Email { get; init; }

    }

    public class CreateUserRequest : IRequest<CreateUserResult>
    {
        public string Email { get; init; }
        public string CreatedById { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
    }

    public class CreateUserValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.CreatedById)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .Equal(x => x.Password).WithMessage("Password and Confirm Password should equal.");
        }
    }


    public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResult>
    {
        private readonly IIdentityService _identityService;

        public CreateUserHandler(
            IIdentityService identityService
            )
        {
            _identityService = identityService;
        }

        public async Task<CreateUserResult> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(
                request.Email,
                request.Password,
                cancellationToken
                );

            return result;
        }
    }
}
