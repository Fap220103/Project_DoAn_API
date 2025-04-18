using Application.Services.Externals;
using FluentValidation;
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
    public class UpdateUserResult
    {
        public string? Id { get; init; }
        public string? Email { get; init; }
        public List<string> Roles { get; init; }
    }

    public class UpdateUserRequest : IRequest<UpdateUserResult>
    {
        public string? Id { get; init; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [FromForm]
        public IFormFile? Image { get; set; } 
        public List<string> Roles { get; set; } = new();
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResult>
    {
        private readonly IIdentityService _identityService;

        public UpdateUserHandler(
            IIdentityService identityService
            )
        {
            _identityService = identityService;
        }

        public async Task<UpdateUserResult> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.UpdateUserAsync(
                request.Id,
                request.UserName,
                request.PhoneNumber,
                request.Image,
                request.Roles,
                cancellationToken
                );

            return result;
        }
    }
}
