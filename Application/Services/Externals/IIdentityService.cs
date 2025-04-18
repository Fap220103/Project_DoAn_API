using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Queries;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IIdentityService
    {
        Task<GetUsersResult> GetUsersAsync(int page, int limit, string? order, string? search, string role, CancellationToken cancellationToken = default);
        Task<CreateUserResult> CreateUserAsync(string email, string password, List<string> roles, CancellationToken cancellationToken = default);
        Task<UpdateUserResult> UpdateUserAsync(string id, string username, string phone, IFormFile image, List<string> roles, CancellationToken cancellationToken = default);
        Task<DeleteUserResult> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<LoginUserResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<LogoutUserResult> LogoutAsync(string userId, CancellationToken cancellationToken = default);
        Task<RegisterUserResult> RegisterUserAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<string> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken = default);
        Task<GenerateRefreshTokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<AddRolesToUserResult> AddRolesToUserAsync(string userId, string[] roles, CancellationToken cancellationToken = default);
        Task<DeleteRolesFromUserResult> DeleteRolesFromUserAsync(string userId, string[] roles, CancellationToken cancellationToken = default);
        Task<ForgotPasswordResult> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<string> ForgotPasswordConfirmationAsync(string email, string tempPassword, string code, CancellationToken cancellationToken = default);
        Task<AddClaimsToRoleResult> AddClaimsToRoleAsync(string role, string[] claims, CancellationToken cancellationToken = default);
        Task<DeleteClaimsFromRoleResult> DeleteClaimsFromRoleAsync(string role, string[] claims, CancellationToken cancellationToken = default);
        Task<GetClaimsByUserResult> GetClaimsByUserAsync(string userId, int pageNumber = 1, int pageSize = 10, string sortBy = "Value", string sortDirection = "asc", string searchValue = "", CancellationToken cancellationToken = default);
        Task<GetClaimsResult> GetClaimsAsync(int pageNumber = 1, int pageSize = 10, string sortBy = "Value", string sortDirection = "asc", string searchValue = "", CancellationToken cancellationToken = default);
        Task<GetClaimsByRoleResult> GetClaimsByRoleAsync(string role, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<CreateRoleResult> CreateRoleAsync(string role, string[] claims, CancellationToken cancellationToken = default);
        Task<DeleteRoleResult> DeleteRoleAsync(string role, CancellationToken cancellationToken = default);
        Task<UpdateRoleResult> UpdateRoleAsync(string oldRole, string newRole, string[] newClaims, CancellationToken cancellationToken = default);
        Task<GetRolesResult> GetRolesAsync(int page = 1, int limit = 10, string sortBy = "Name", string sortDirection = "asc", string searchValue = "", CancellationToken cancellationToken = default);
        Task<GetRolesByUserResult> GetRolesByUserAsync(string userId, int page = 1, int limit = 10, CancellationToken cancellationToken = default);
    }
}
