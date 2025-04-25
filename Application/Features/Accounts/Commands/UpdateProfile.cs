using Application.Services.Externals;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands
{
    public class UpdateProfileResult
    {
        public string? Id { get; init; }
        public string? Email { get; init; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UpdateProfileRequest : IRequest<UpdateProfileResult>
    {
        public string Id { get; init; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        [FromForm]
        public IFormFile? Image { get; set; }
    }

    public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest, UpdateProfileResult>
    {
        private readonly IIdentityService _identityService;

        public UpdateProfileHandler(
            IIdentityService identityService
            )
        {
            _identityService = identityService;
        }

        public async Task<UpdateProfileResult> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateProfileAsync(
                request.Id,
                request.UserName,
                request.PhoneNumber,
                request.Image,
                cancellationToken
                );

            return result;
        }
    }
}
