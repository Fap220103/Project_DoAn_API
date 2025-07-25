﻿using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Commands
{
    public class DeleteRolesFromUserResult
    {
        public string? UserId { get; init; }
        public string[] Roles { get; init; } = Array.Empty<string>();
    }

    public class DeleteRolesFromUserRequest : IRequest<DeleteRolesFromUserResult>
    {
        [Required]
        public string UserId { get; init; }
        public string[] Roles { get; init; } = Array.Empty<string>();
    }

    public class DeleteRolesFromUserValidator : AbstractValidator<DeleteRolesFromUserRequest>
    {
        public DeleteRolesFromUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Roles)
                .NotEmpty();
        }
    }

    public class DeleteRolesFromUserHandler : IRequestHandler<DeleteRolesFromUserRequest, DeleteRolesFromUserResult>
    {
        private readonly IIdentityService _identityService;

        public DeleteRolesFromUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DeleteRolesFromUserResult> Handle(DeleteRolesFromUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.DeleteRolesFromUserAsync(
                request.UserId,
                request.Roles,
                cancellationToken
                );

            return result;
        }
    }

}
