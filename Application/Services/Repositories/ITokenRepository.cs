using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Repositories
{
    public interface ITokenRepository : IBaseCommandRepository<Token>
    {
        Task<Token> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<List<Token>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    }
}
