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
    public class DeleteUserResult
    {
        public string? Id { get; init; }
        public string? Email { get; init; }

    }

    public class DeleteUserRequest : IRequest<DeleteUserResult>
    {
        public string UserId { get; init; }
    }

    public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }

    public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResult>
    {
        private readonly IIdentityService _identityService;

        public DeleteUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<DeleteUserResult> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.DeleteUserAsync(request.UserId, cancellationToken);

            return result;
        }
    }

}
