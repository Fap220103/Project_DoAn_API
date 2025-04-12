using Application.Features.NavigationManagers.Queries;
using Application.Services.Externals;
using Infrastructure.SecurityManagers.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.Navigations
{
    public class NavigationService : INavigationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleClaimService _roleClaimService;

        public NavigationService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRoleClaimService roleClaimService
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleClaimService = roleClaimService;
        }

        private List<TDestination> MapResult<TSource, TDestination>(
                List<TSource> sourceItems,
                Func<TSource, TDestination> mapFunc,
                Func<TSource, IEnumerable<TSource>> getChildrenFunc,
                Action<TDestination, List<TDestination>> setChildrenAction
            )
        {
            return sourceItems.Select(item =>
            {
                var result = mapFunc(item);
                var children = getChildrenFunc(item).Select(child => mapFunc(child)).ToList();
                setChildrenAction(result, children);
                return result;
            }).ToList();
        }

        private bool CheckAuthorization(IEnumerable<Claim> claims, string navigationName)
        {
            var requiredPermissions = new List<string>
        {
            $"{navigationName}:Create",
            $"{navigationName}:Read",
            $"{navigationName}:Update",
            $"{navigationName}:Delete"
        };

            return claims.Any(c => requiredPermissions.Contains(c.Value));
        }


        private async Task<List<NavigationItem>> BuildMainNav(string userId)
        {
            var navItems = new List<NavigationItem>();
            var claims = await _roleClaimService.GetClaimListByUserAsync(userId);
            var finalNavigations = NavigationBuilder.BuildFinalNavigations();
            foreach (var navigation in finalNavigations)
            {
                var parentNav = new NavigationItem(
                    navigation.ParentName,
                    navigation.ParentCaption,
                    navigation.ParentUrl
                    );
                foreach (var child in navigation.Children)
                {
                    var isAuthorized = CheckAuthorization(claims, child.Name);

                    if (isAuthorized) 
                    {
                        parentNav.AddChild(new NavigationItem(
                            child.Name,
                            child.Caption,
                            child.Url,
                            isAuthorized
                        ));
                    }
                }

                parentNav.IsAuthorized = parentNav.Children.Any(x => x.IsAuthorized);
                navItems.Add(parentNav);
            }

            return navItems.Where(x => x.IsAuthorized).ToList();
        }

        public async Task<GetMainNavResult> GenerateMainNavAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NavigationException($"Invalid userid: {userId}");
            }

            var navItems = await BuildMainNav(user.Id);

            var results = MapResult(
                    navItems,
                    item => new MainNavDto(item.Name, item.Caption, item.Url, item.IsAuthorized),
                    item => item.Children,
                    (parent, children) => parent.Children = children
                );

            cancellationToken.ThrowIfCancellationRequested();
            return new GetMainNavResult { MainNavigations = results };
        }
    }

}
