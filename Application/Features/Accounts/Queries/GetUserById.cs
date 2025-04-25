using Application.Common.Models;
using Application.Features.Accounts.Dtos;
using Application.Services.Externals;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Queries
{
    public class GetUserByIdResult
    {
        public ApplicationUserDto Data { get; init; }
        public string Message { get; init; } = null!;
    }
    public class GetUserByIdRequest : IRequest<GetUserByIdResult>
    {
        public string userId { get; init; }
    }
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResult>
    {
        private readonly IIdentityService _identityService;

        public GetUserByIdHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetUserByIdResult> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetUserByIdAsync(
                                    request.userId,
                                    cancellationToken);

            return new GetUserByIdResult
            {
                Data = result,
                Message = "Success"
            };
        }
    }
}
