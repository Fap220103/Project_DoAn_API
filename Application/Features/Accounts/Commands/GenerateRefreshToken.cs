using Application.Features.NavigationManagers.Queries;
using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands
{
    public class GenerateRefreshTokenResult
    {
        public string? AccessToken { get; init; }
        [JsonIgnore]
        public string? RefreshToken { get; init; }
        public int expires_in_second { get; set; }
    }

    public class GenerateRefreshTokenRequest : IRequest<GenerateRefreshTokenResult>
    {
        public string RefreshToken { get; set; }
    }

    public class GenerateRefreshTokenValidator : AbstractValidator<GenerateRefreshTokenRequest>
    {
        public GenerateRefreshTokenValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty();
        }
    }

    public class GenerateRefreshTokenHandler : IRequestHandler<GenerateRefreshTokenRequest, GenerateRefreshTokenResult>
    {
        private readonly IIdentityService _identityService;

        public GenerateRefreshTokenHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GenerateRefreshTokenResult> Handle(GenerateRefreshTokenRequest request, CancellationToken cancellationToken)
        {

            var result = await _identityService.RefreshTokenAsync(
                request.RefreshToken,
                cancellationToken
                );

            return result;
        }
    }
}
