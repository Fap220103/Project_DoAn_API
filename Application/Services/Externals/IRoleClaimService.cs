using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IRoleClaimService
    {
        Task<List<Claim>> GetClaimListByUserAsync(
            string userId,
            CancellationToken cancellationToken = default);
        Task<List<Claim>> GetClaimListAsync(
            CancellationToken cancellationToken = default);
    }

}
