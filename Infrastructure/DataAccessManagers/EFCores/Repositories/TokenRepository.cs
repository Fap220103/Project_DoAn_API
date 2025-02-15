using Application.Services.Repositories;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Infrastructure.DataAccessManagers.EFCores.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Repositories
{
    public class TokenRepository : BaseCommandRepository<Token>, ITokenRepository
    {
        public TokenRepository(CommandContext context) : base(context)
        {
        }
        public async Task<Token> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var entity = await _context.Token
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken, cancellationToken);

            if (entity == null)
            {
                throw new TokenRepositoryException($"Refresh token has expired, please re-login. {refreshToken}");
            }
            return entity;
        }
        public async Task<List<Token>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            var entities = await _context.Token
                .Where(x => x.UserId == userId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities;
        }
    }

}
