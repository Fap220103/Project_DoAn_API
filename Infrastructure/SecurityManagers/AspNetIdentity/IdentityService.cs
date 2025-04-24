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
using Domain.Interfaces;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INavigationService _navigationService;
        private readonly QueryContext _queryContext;
        private readonly IPhotoService _photoService;

        public IdentityService(
            IOptions<IdentitySettings> identitySettings,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            ITokenRepository tokenRepository,
            IUnitOfWork unitOfWork,
            INavigationService navigationService,
            QueryContext queryContext,
            IPhotoService photoService
            )
        {
            _identitySettings = identitySettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _navigationService = navigationService;
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

            if(!string.IsNullOrEmpty(user.ProfilePictureName))
            {
                var deleteResult = await _photoService.DeletePhotoAsync(user.ProfilePictureName);
                if (deleteResult == null)
                    throw new ApplicationException("Xóa ảnh không thành công");
            }
          

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
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                throw new IdentityException("Email đã được sử dụng.");
            }
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

        public async Task<string?> GetCustomerNameAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var user = await _queryContext.Users
                .Where(u => u.Id == customerId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }
    }

}
