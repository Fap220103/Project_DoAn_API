using Application.Common.Models;
using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Dtos;
using Application.Features.Accounts.Queries;
using Application.Features.ProductCategories.Queries;
using Application.Services.Externals;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Infrastructure.SecurityManagers.RoleClaims;
using Infrastructure.SecurityManagers.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SecurityManagers.AspNetIdentity
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentitySettings _identitySettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INavigationService _navigationService;
        private readonly IRoleClaimService _roleClaimService;
        private readonly QueryContext _queryContext;
        private readonly IPhotoService _photoService;

        public IdentityService(
            IOptions<IdentitySettings> identitySettings,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ITokenRepository tokenRepository,
            IUnitOfWork unitOfWork,
            INavigationService navigationService,
            IRoleClaimService roleClaimService,
            QueryContext queryContext,
            IPhotoService photoService
            )
        {
            _identitySettings = identitySettings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
            _roleClaimService = roleClaimService;
            _queryContext = queryContext;
            _photoService = photoService;
        }

        public async Task<CreateUserResult> CreateUserAsync(string email, string password, List<string> roles, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var checkUser = await _userManager.FindByEmailAsync(email);
            if(checkUser != null)
            {
                throw new IdentityException($"{ExceptionConsts.EmailAlreadyExists} {email}");
            }
            var user = new ApplicationUser(email);

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new IdentityException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            foreach (var role in roles)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!await _userManager.IsInRoleAsync(user, role))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, role);
                    if (!addRoleResult.Succeeded)
                    {
                        var errorMessages = string.Join("; ", addRoleResult.Errors.Select(e => e.Description));
                        throw new IdentityException($"Error adding role '{role}' to user: {errorMessages}");
                    }
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new CreateUserResult
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles,
            };
        }

        public async Task<DeleteUserResult> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new IdentityException($"User not found. ID: {userId}");
            }

            var tokens = _queryContext.Token.Where(t => t.UserId == userId).ToList();
            _queryContext.Token.RemoveRange(tokens);
            _queryContext.SaveChanges();
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                throw new IdentityException(string.Join(", ", result.Errors.Select(e => e.Description)));

            }

            cancellationToken.ThrowIfCancellationRequested();

            return new DeleteUserResult
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        public async Task<GetUsersResult> GetUsersAsync(int page, int limit, string order, string search, string? role = null, CancellationToken cancellationToken = default)
        {
            var query = _queryContext.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(role))
            {
                var userIdsWithRole = (await _userManager.GetUsersInRoleAsync(role))
                                                    .Select(u => u.Id)
                                                    .ToList();
                query = query.Where(u => userIdsWithRole.Contains(u.Id));
            }
            // Tìm kiếm theo tên hoặc email
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchKeyword = search.Trim().ToLower();
                query = query.Where(u =>
                    u.UserName.ToLower().Contains(searchKeyword) ||
                    u.Email.ToLower().Contains(searchKeyword)
                );
            }

            // Sắp xếp nếu có order
            if (!string.IsNullOrEmpty(order))
            {
                var parts = order.Split('|');
                if (parts.Length == 2)
                {
                    var field = parts[0].ToLower();
                    var direction = parts[1].ToLower();

                    query = (field, direction) switch
                    {
                        ("username", "asc") => query.OrderBy(x => x.UserName),
                        ("username", "desc") => query.OrderByDescending(x => x.UserName),
                        ("email", "asc") => query.OrderBy(x => x.Email),
                        ("email", "desc") => query.OrderByDescending(x => x.Email),
                        _ => query
                    };
                }
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt); 
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(u => new ApplicationUserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    ProfilePictureName = u.ProfilePictureName,
                    EmailConfirmed = u.EmailConfirmed,
                    Status = u.Status,
                    IsBlocked = u.IsBlocked,
                    CreatedAt = u.CreatedAt,
                    Roles = null
                })
                .ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                var user = await _userManager.FindByIdAsync(item.Id);
                item.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            }

            var pagedList = new PagedList<ApplicationUserDto>(items, total, page, limit);

            return new GetUsersResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }

        public async Task<LoginUserResult> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid login credentials.");
            }

            if (user.IsBlocked)
            {
                throw new UnauthorizedAccessException($"User is blocked. {email}");
            }

            if (user.IsDeleted)
            {
                throw new UnauthorizedAccessException($"User already deleted. {email}");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false);
            //var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.IsLockedOut)
            {
                throw new UnauthorizedAccessException("Invalid login credentials. IsLockedOut.");
            }

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid login credentials. NotSucceeded.");
            }


            var mainNavs = await _navigationService.GenerateMainNavAsync(user.Id, cancellationToken);
            var accessToken = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var tokens = await _tokenRepository.GetByUserIdAsync(user.Id, cancellationToken);
            foreach (var tokenItem in tokens)
            {
                _tokenRepository.Purge(tokenItem);
            }

            var token = new Token(user.Id, refreshToken);
            await _tokenRepository.CreateAsync(token, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new LoginUserResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email,
                MainNavigations = mainNavs.MainNavigations
            };
        }

        public async Task<LogoutUserResult> LogoutAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new IdentityException($"Invalid userId: {userId}");
            }

            var tokens = await _tokenRepository.GetByUserIdAsync(userId, cancellationToken);
            foreach (var token in tokens)
            {
                _tokenRepository.Purge(token);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return new LogoutUserResult { UserId = userId };
        }

        public async Task<RegisterUserResult> RegisterUserAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new ApplicationUser(
                email
            );
            user.UserName = email;
            var sendEmailConfirmation = false;
            if (_identitySettings.SignIn.RequireConfirmedEmail)
            {
                //email confirmation should be sent to confirmed the registered email.
                user.EmailConfirmed = false;
                sendEmailConfirmation = true;
            }
            else
            {
                user.EmailConfirmed = true;
            }
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new IdentityException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await RoleClaimHelper.AddBasicRoleToUser(_userManager, user);

            cancellationToken.ThrowIfCancellationRequested();

            return new RegisterUserResult
            {
                Id = user.Id,
                Email = email,
                UserName = email,
                EmailConfirmationToken = code,
                SendEmailConfirmation = sendEmailConfirmation
            };
        }
        public async Task<string> ConfirmEmailAsync(string email, string code, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new IdentityException($"Unable to load user with email: {email}");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                throw new IdentityException($"Error confirming your email: {email}");
            }

            return email;
        }
        public async Task<GenerateRefreshTokenResult> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {

            var token = await _tokenRepository.GetByRefreshTokenAsync(refreshToken, cancellationToken);

            if (token.ExpiryDate < DateTime.UtcNow)
            {
                throw new IdentityException("Refresh token has expired, please re-login");
            }

            var user = await _userManager.FindByIdAsync(token.UserId);

            if (user == null)
            {
                throw new IdentityException("Refresh token has expired, please re-login");
            }
            var newAccessToken = await _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _tokenRepository.Purge(token);

            var newToken = new Token(user.Id, newRefreshToken);
            await _tokenRepository.CreateAsync(newToken, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return new GenerateRefreshTokenResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }
        public async Task<AddRolesToUserResult> AddRolesToUserAsync(string userId, string[] roles, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new IdentityException($"User not found. ID: {userId}");
            }

            foreach (var role in roles)
            {
                if (!await _userManager.IsInRoleAsync(user, role))
                {
                    var result = await _userManager.AddToRoleAsync(user, role);
                    if (!result.Succeeded)
                    {
                        var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                        throw new IdentityException($"Error adding role '{role}' to user: {errorMessages}");
                    }

                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            cancellationToken.ThrowIfCancellationRequested();

            return new AddRolesToUserResult
            {
                UserId = userId,
                Roles = userRoles.ToArray()
            };
        }
        public async Task<DeleteRolesFromUserResult> DeleteRolesFromUserAsync(string userId, string[] roles, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new IdentityException($"User not found. ID: {userId}");
            }

            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    var result = await _userManager.RemoveFromRoleAsync(user, role);
                    if (!result.Succeeded)
                    {
                        var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                        throw new IdentityException($"Error deleting role '{role}' from user: {errorMessages}");
                    }

                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            cancellationToken.ThrowIfCancellationRequested();

            return new DeleteRolesFromUserResult
            {
                UserId = userId,
                Roles = userRoles.ToArray()
            };
        }
        public async Task<ForgotPasswordResult> ForgotPasswordAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new IdentityException($"Unable to load user with email: {email}");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var clearTempPassword = Guid.NewGuid().ToString().Substring(0, _identitySettings.Password.RequiredLength);
            var tempPassword = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(clearTempPassword));

            cancellationToken.ThrowIfCancellationRequested();

            return new ForgotPasswordResult
            {
                Email = email,
                TempPassword = tempPassword,
                EmailConfirmationToken = code,
                ClearTempPassword = clearTempPassword
            };
        }

        public async Task<string> ForgotPasswordConfirmationAsync(
            string email,
            string tempPassword,
            string code,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new IdentityException($"Unable to load user with email: {email}");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            tempPassword = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(tempPassword));
            var result = await _userManager.ResetPasswordAsync(user, code, tempPassword);

            if (!result.Succeeded)
            {
                throw new IdentityException($"Error resetting your password");
            }

            return email;
        }
        public async Task<AddClaimsToRoleResult> AddClaimsToRoleAsync(string role, string[] claims, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var identityRole = await _roleManager.FindByNameAsync(role);

            if (identityRole == null)
            {
                throw new IdentityException($"Role '{role}' not found.");
            }

            if (!RoleClaimHelper.GetPermissionClaims().Any(x => claims.Contains(x)))
            {
                throw new IdentityException($"Contain not valid claims: {string.Join("; ", claims.Select(x => x))}.");
            }

            foreach (var claim in claims)
            {
                var claimToAdd = new Claim("Permission", claim);
                var result = await _roleManager.AddClaimAsync(identityRole, claimToAdd);

                if (!result.Succeeded)
                {
                    throw new IdentityException($"Failed to add claim '{claim}' to role '{role}'.");
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new AddClaimsToRoleResult
            {
                Role = role,
                Claims = claims
            };
        }

        public async Task<DeleteClaimsFromRoleResult> DeleteClaimsFromRoleAsync(string role, string[] claims, CancellationToken cancellationToken = default)
        {

            cancellationToken.ThrowIfCancellationRequested();

            var identityRole = await _roleManager.FindByNameAsync(role);

            if (identityRole == null)
            {
                throw new IdentityException($"Role '{role}' not found.");
            }

            if (!RoleClaimHelper.GetPermissionClaims().Any(x => claims.Contains(x)))
            {
                throw new IdentityException($"Contain not valid claims: {string.Join("; ", claims.Select(x => x))}.");
            }

            foreach (var claim in claims)
            {
                var claimToDelete = new Claim("Permission", claim);
                var result = await _roleManager.RemoveClaimAsync(identityRole, claimToDelete);

                if (!result.Succeeded)
                {
                    throw new IdentityException($"Failed to delete claim '{claim}' from role '{role}'.");
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new DeleteClaimsFromRoleResult
            {
                Role = role,
                Claims = claims
            };
        }
        public async Task<GetClaimsByUserResult> GetClaimsByUserAsync(
            string userId,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Value",
            string sortDirection = "asc",
            string searchValue = "",
            CancellationToken cancellationToken = default)
        {

            var claims = await _roleClaimService.GetClaimListByUserAsync(userId, cancellationToken);
            int counter = 1;
            var userClaims = claims.Select(x => new ClaimDto { Id = counter++.ToString(), Type = x.Type, Value = x.Value }).ToList();

            if (
                searchValue is not null
                && searchValue != "null"
                && !string.IsNullOrWhiteSpace(searchValue.Trim())
                )
            {
                userClaims = userClaims
                    .Where(x => x.Type != null && x.Type.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                 x.Value != null && x.Value.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }


            if (sortBy.Equals("Value", StringComparison.OrdinalIgnoreCase))
            {
                userClaims = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? userClaims.OrderBy(x => x.Value).ToList()
                    : userClaims.OrderByDescending(x => x.Value).ToList();
            }

            return new GetClaimsByUserResult { Claims = PagedList<ClaimDto>.FromList(userClaims, pageNumber, pageSize) };
        }
        public async Task<GetClaimsResult> GetClaimsAsync(
           int pageNumber = 1,
           int pageSize = 10,
           string sortBy = "Value",
           string sortDirection = "asc",
           string searchValue = "",
           CancellationToken cancellationToken = default)
        {

            var claims = await _roleClaimService.GetClaimListAsync(cancellationToken);

            int counter = 1;
            var userClaims = claims.Select(x => new ClaimDto
            {
                Id = counter++.ToString(),
                Type = x.Type,
                Value = x.Value
            }).ToList();

            if (
                searchValue is not null
                && searchValue != "null"
                && !string.IsNullOrWhiteSpace(searchValue.Trim())
                )
            {
                userClaims = userClaims
                    .Where(x => x.Type != null && x.Type.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                 x.Value != null && x.Value.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }


            if (sortBy.Equals("Value", StringComparison.OrdinalIgnoreCase))
            {
                userClaims = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? userClaims.OrderBy(x => x.Value).ToList()
                    : userClaims.OrderByDescending(x => x.Value).ToList();
            }

            return new GetClaimsResult { Data = PagedList<ClaimDto>.FromList(userClaims, pageNumber, pageSize) };
        }
        public async Task<GetClaimsByRoleResult> GetClaimsByRoleAsync(
           string role,
           int pageNumber = 1,
           int pageSize = 10,
           CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roleIdentity = await _roleManager.FindByNameAsync(role);
            if (roleIdentity == null)
            {
                throw new IdentityException($"Role not found. Role: {role}");
            }

            var roleClaims = new List<string>();
            var claimsForRole = await _roleManager.GetClaimsAsync(roleIdentity);
            roleClaims.AddRange(claimsForRole.Select(x => x.Value).ToList());

            cancellationToken.ThrowIfCancellationRequested();

            return new GetClaimsByRoleResult { Claims = PagedList<string>.FromList(roleClaims, pageNumber, pageSize) };
        }
        public async Task<CreateRoleResult> CreateRoleAsync(
            string role,
            string[] claims,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var newRole = new IdentityRole(role);
                var identityResult = await _roleManager.CreateAsync(newRole);
                if (!identityResult.Succeeded)
                {
                    throw new IdentityException($"CreateRolesAsync error: {identityResult.Errors.Select(e => e.Description)}");
                }

                foreach (var claim in claims)
                {
                    var addResult = await _roleManager.AddClaimAsync(newRole, new Claim("Permission", claim));
                    if (!addResult.Succeeded)
                    {
                        throw new IdentityException("Error adding new claims to the role.");
                    }
                }

            }
            else
            {
                throw new IdentityException($"Role {role} already exists.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new CreateRoleResult { Role = role, Claims = claims };
        }

        public async Task<DeleteRoleResult> DeleteRoleAsync(
            string role,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var deletedRole = await _roleManager.FindByNameAsync(role);
            if (deletedRole != null)
            {
                // Remove all claims associated with the role
                var claims = await _roleManager.GetClaimsAsync(deletedRole);
                foreach (var claim in claims)
                {
                    var removeClaimResult = await _roleManager.RemoveClaimAsync(deletedRole, claim);
                    if (!removeClaimResult.Succeeded)
                    {
                        throw new IdentityException("Error removing claims from the role.");
                    }
                }

                // Delete the role
                var result = await _roleManager.DeleteAsync(deletedRole);
                if (!result.Succeeded)
                {
                    throw new IdentityException($"Error deleting role {role}.");
                }
            }
            else
            {
                throw new IdentityException($"Role {role} not found.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new DeleteRoleResult { };
        }
        public async Task<UpdateRoleResult> UpdateRoleAsync(
        string oldRole,
        string newRole,
        string[] newClaims,
        CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Find the role by its old name
            var role = await _roleManager.FindByNameAsync(oldRole)
                ?? throw new IdentityException($"Role {oldRole} not found.");

            // If the role name is changing, update it
            if (oldRole != newRole)
            {
                role.Name = newRole;
                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    throw new IdentityException($"Error: Update role name from {oldRole} to {newRole}");
                }
            }

            // Remove existing claims
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in existingClaims)
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                {
                    throw new IdentityException("Error removing existing claims from the role.");
                }
            }

            // Add new claims
            foreach (var claim in newClaims)
            {
                var addResult = await _roleManager.AddClaimAsync(role, new Claim("Permission", claim));
                if (!addResult.Succeeded)
                {
                    throw new IdentityException("Error adding new claims to the role.");
                }
            }

            cancellationToken.ThrowIfCancellationRequested();

            return new UpdateRoleResult { Role = newRole, Claims = newClaims };
        }
        public async Task<GetRolesResult> GetRolesAsync(
            int page = 1,
            int limit = 10,
            string sortBy = "Name",
            string sortDirection = "asc",
            string searchValue = "",
            CancellationToken cancellationToken = default)
        {

            cancellationToken.ThrowIfCancellationRequested();

            int counter = 1;
            var roles = _roleManager.Roles
                                    .Select(r => r.Name)
                                    .Where(name => name != null)
                                    .ToList()
                                    .Select(name => new RoleDto
                                    {
                                        Id = counter++.ToString(),
                                        Name = name
                                    })
                                    .ToList();

            if (
                searchValue is not null
                && searchValue != "null"
                && !string.IsNullOrWhiteSpace(searchValue.Trim())
                )
            {
                roles = roles
                    .Where(x => x.Name != null && x.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                roles = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                    ? roles.OrderBy(x => x.Name).ToList()
                    : roles.OrderByDescending(x => x.Name).ToList();
            }

            foreach (var role in roles)
            {
                if (role != null && role.Name != null)
                {
                    var identityRole = await _roleManager.FindByNameAsync(role.Name);
                    if (identityRole != null)
                    {
                        var claims = await _roleManager.GetClaimsAsync(identityRole);
                        role.Claims = claims.Select(c => c.Value).ToList();
                    }
                }
            }

            await Task.CompletedTask;

            cancellationToken.ThrowIfCancellationRequested();

            return new GetRolesResult { Data = PagedList<RoleDto>.FromList(roles, page, limit) };
        }
        public async Task<GetRolesByUserResult> GetRolesByUserAsync(
           string userId,
           int page = 1,
           int limit = 10,
           CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new IdentityException($"User not found. ID: {userId}");
            }

            List<Claim> userClaims = (await _userManager.GetClaimsAsync(user)).ToList();

            var roles = await _userManager.GetRolesAsync(user);

            cancellationToken.ThrowIfCancellationRequested();

            return new GetRolesByUserResult { Roles = PagedList<string>.FromList(roles.ToList(), page, limit) };
        }

        public async Task<UpdateUserResult> UpdateUserAsync(string id, string username, string phone, int status, IFormFile image, List<string> roles, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new IdentityException($"User not found. ID: {id}");

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
                throw new IdentityException($"Không thể xóa role hiện tại");

            var addRolesResult = await _userManager.AddToRolesAsync(user, roles);
            if (!addRolesResult.Succeeded)
                throw new IdentityException($"Không thể thêm role mới");


            string imageUrl = null;
           
            if (image != null && image.Length > 0)
            {
                string existingImageUrl = user.ProfilePictureName;
                if (!string.IsNullOrEmpty(existingImageUrl))
                {
                    var deleteResult = await _photoService.DeletePhotoAsync(existingImageUrl);
                    if (deleteResult==null)
                        throw new IdentityException("Xóa ảnh không thành công");
                }
                var uploadResult = await _photoService.AddPhotoAsync(image);
                if (uploadResult == null)
                    throw new IdentityException("Tải ảnh lên không thành công");

                imageUrl = uploadResult.Url.ToString();
            }
            else
            {
                imageUrl = user.ProfilePictureName;
            }
            user.UserName = username;
            if(!string.IsNullOrEmpty(phone))
            {
                user.PhoneNumber = phone;
            }

            user.ProfilePictureName = imageUrl;
            user.Status = status;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)  
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new IdentityException($"Cập nhật user thất bại: {errorMessages}");
            }

            return new UpdateUserResult
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles,
            };
        }
    }

}
