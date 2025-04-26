using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Dtos;
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
        Task<UpdateUserResult> UpdateUserAsync(string id, string? username, string? phone, int status, IFormFile? image, List<string>? roles, CancellationToken cancellationToken = default);
        Task<DeleteUserResult> DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
        Task<LoginUserResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<LogoutUserResult> LogoutAsync(string userId, CancellationToken cancellationToken = default);
        Task<RegisterUserResult> RegisterUserAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<string> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken = default);
        Task<GenerateRefreshTokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
        Task<ForgotPasswordResult> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default);
        Task<string> ForgotPasswordConfirmationAsync(string email, string tempPassword, string code, CancellationToken cancellationToken = default);
        Task<string> ResetPasswordAsync(string email, string newPassword, string code, CancellationToken cancellationToken = default);
        Task<string?> GetCustomerNameAsync(string customerId, CancellationToken cancellationToken = default);
        Task<bool> IsUserExistsAsync(string userId, CancellationToken cancellationToken = default);
        Task<ApplicationUserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<UpdateProfileResult> UpdateProfileAsync(string id, string username, string phone, IFormFile image, CancellationToken cancellationToken = default);
        Task<ChangePassResult> ChangePasswordAsync(string currentPass, string newPass, string userId, CancellationToken cancellationToken = default);
    }
}
