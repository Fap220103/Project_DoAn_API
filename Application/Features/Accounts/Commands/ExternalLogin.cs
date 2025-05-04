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
    public class ExternalLoginResult
    {
        public string? AccessToken { get; init; }
        [JsonIgnore]
        public string? RefreshToken { get; init; }
        public int expires_in_second { get; set; }
        public string? UserId { get; init; }
        public string? Email { get; init; }
        public List<MainNavDto>? MainNavigations { get; init; }
    }

    public class ExternalLoginRequest : IRequest<ExternalLoginResult>
    {
        public string IdToken { get; set; }
    }


    public class ExternalLoginValidator : AbstractValidator<ExternalLoginRequest>
    {
        public ExternalLoginValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty();

        }
    }

    public class ExternalLoginHandler : IRequestHandler<ExternalLoginRequest, ExternalLoginResult>
    {
        private readonly IIdentityService _identityService;

        public ExternalLoginHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ExternalLoginResult> Handle(ExternalLoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ExternalLoginAsync(
                request.IdToken,
                cancellationToken
                );

            return result;
        }
    }
}
