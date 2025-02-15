using Application.Features.NavigationManagers.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface INavigationService
    {
        Task<GetMainNavResult> GenerateMainNavAsync(string userId, CancellationToken cancellationToken = default);
    }
}
