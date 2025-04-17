using Application.Features.NavigationManagers.Queries;
using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands
{
    public class LoginUserResult
    {
        public string? AccessToken { get; init; }
        [JsonIgnore]
        public string? RefreshToken { get; init; }
        public int expires_in_second { get; set; }
        public string? UserId { get; init; }
        public string? Email { get; init; }
        public List<MainNavDto>? MainNavigations { get; init; }
    }

    public class LoginUserRequest : IRequest<LoginUserResult>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }


    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }

    public class LoginUserHandler : IRequestHandler<LoginUserRequest, LoginUserResult>
    {
        private readonly IIdentityService _identityService;

        public LoginUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginUserResult> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.LoginAsync(
                request.Email,
                request.Password,
                cancellationToken
                );

            return result;
        }
    }
}
