using Application.Services.Externals;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.NavigationManagers.Queries
{
    public class MainNavDto
    {
        public string Name { get; init; }
        public string Caption { get; init; }
        public string Url { get; init; }
        public bool IsAuthorized { get; init; }
        public List<MainNavDto> Children { get; set; } = new List<MainNavDto>();
        public bool expanded { get; init; } = false;

        public MainNavDto(
            string name,
            string caption,
            string url,
            bool isAuthorized = false
            )
        {
            Name = name;
            Caption = caption;
            Url = url;
            IsAuthorized = isAuthorized;
        }

        public void AddChild(MainNavDto child)
        {
            Children.Add(child);
        }
    }

    public class GetMainNavResult
    {
        public List<MainNavDto>? MainNavigations { get; init; }
    }

    public class GetMainNavRequest : IRequest<GetMainNavResult>
    {
        public string UserId { get; init; }
    }

    public class GetMainNavValidator : AbstractValidator<GetMainNavRequest>
    {
        public GetMainNavValidator()
        {
        }
    }

    public class GetMainNavHandler : IRequestHandler<GetMainNavRequest, GetMainNavResult>
    {
        private readonly INavigationService _navigationService;

        public GetMainNavHandler(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<GetMainNavResult> Handle(GetMainNavRequest request, CancellationToken cancellationToken)
        {
            var result = await _navigationService.GenerateMainNavAsync(request.UserId, cancellationToken);

            return result;
        }
    }
}
