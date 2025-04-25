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
    public class ChangePassResult
    {
        public string userId { get; init; } = null!;
        public string Message { get; init; } = null!;
        public bool Success { get; set; }
    }

    public class ChangePassRequest : IRequest<ChangePassResult>
    {
        public string CurrentPass { get; init; }
        public string NewPass { get; init; }
        public string UserId { get; init; }
    }

    public class ChangePassValidator : AbstractValidator<ChangePassRequest>
    {
        public ChangePassValidator()
        {
            RuleFor(x => x.CurrentPass)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPass)
                .NotEmpty();
        }
    }


    public class ChangePassHandler : IRequestHandler<ChangePassRequest, ChangePassResult>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public ChangePassHandler(
            IMediator mediator,
            IIdentityService identityService
            )
        {
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<ChangePassResult> Handle(ChangePassRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangePasswordAsync(
                request.CurrentPass,
                request.NewPass,
                request.UserId,
                cancellationToken
                );

            return result;
        }
    }
}
