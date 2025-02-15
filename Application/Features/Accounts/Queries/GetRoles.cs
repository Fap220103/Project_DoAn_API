using Application.Common.Models;
using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Accounts.Queries
{
    public class RoleDto
    {
        public string? Id { get; set; }
        public string? Name { get; init; }
        public IList<string> Claims { get; set; } = new List<string>();

    }

    public class GetRolesResult
    {
        public PagedList<RoleDto>? Data { get; init; }
        public string Message { get; init; } = null!;
    }

    public class GetRolesRequest : IRequest<GetRolesResult>
    {
        public int pageNumber { get; init; }
        public int pageSize { get; init; }
        public string SortBy { get; init; }
        public string SortDirection { get; init; }
        public string searchValue { get; init; } = string.Empty;
    }

    public class GetRolesValidator : AbstractValidator<GetRolesRequest>
    {
        public GetRolesValidator()
        {
            RuleFor(x => x.pageNumber)
                .NotEmpty();

            RuleFor(x => x.pageSize)
                .NotEmpty();

            RuleFor(x => x.SortBy)
                .NotEmpty();

            RuleFor(x => x.SortDirection)
                .NotEmpty();
        }
    }


    public class GetRolesHandler : IRequestHandler<GetRolesRequest, GetRolesResult>
    {
        private readonly IIdentityService _identityService;

        public GetRolesHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetRolesResult> Handle(GetRolesRequest request, CancellationToken cancellationToken)
        {
            var result = await _identityService.GetRolesAsync(
                request.pageNumber,
                request.pageSize,
                request.SortBy,
                request.SortDirection,
                request.searchValue,
                cancellationToken);

            return new GetRolesResult
            {
                Data = result.Data,
                Message = "Success"
            };
        }
    }
}
