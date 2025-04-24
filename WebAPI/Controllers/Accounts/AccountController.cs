using Application.Features.Accounts.Commands;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using WebAPI.Common.Exceptions;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Accounts
{
    public class AccountController : BaseApiController
    {
        private readonly IConfiguration _configuration;
        public AccountController(ISender sender, IConfiguration configuration) : base(sender)
        {
            _configuration = configuration;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ApiSuccessResult<RegisterUserResult>>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<RegisterUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(RegisterUserAsync)}",
                Content = response
            });
        }
        [HttpGet("CheckLoginStatus")]
        public IActionResult CheckLoginStatusAsync()
        {
            if (User.Identity == null)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized: Token not valid or expired. Please re-login"
                    );
            }

            if (!User.Identity.IsAuthenticated)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized: Token not valid or expired. Please re-login"
                    );
            }

            return Ok(new ApiSuccessResult<string>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CheckLoginStatusAsync)}",
                Content = $"UserId: {User.FindFirst(ClaimTypes.NameIdentifier)?.Value}"
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiSuccessResult<LoginUserResult>>> LoginAsync(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            if (response == null)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Invalid or expired token"
                    );
            }

            var accessToken = response.AccessToken;
            var refreshToken = response.RefreshToken;

            if (accessToken == null || refreshToken == null)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Access token or refresh token is null"
                    );
            }

            var refreshTokenCookieName = _configuration["Jwt:refreshTokenCookieName"];
            double expireInMinute;
            if (!double.TryParse(_configuration["Jwt:ExpireInMinute"], out expireInMinute))
            {
                expireInMinute = 15.0;
            }
            response.expires_in_second = (int)(expireInMinute * 60);
            if (refreshTokenCookieName != null)
            {
                HttpContext.Response.Cookies.Delete(refreshTokenCookieName);
                HttpContext.Response.Cookies.Append(refreshTokenCookieName, refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(TokenConsts.ExpiryInDays)
                });

            }

            return Ok(new ApiSuccessResult<LoginUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(LoginAsync)}",
                Content = response
            });
        }
        [HttpPost("Logout")]
        public async Task<ActionResult<ApiSuccessResult<LogoutUserResult>>> LogoutAsync([FromQuery]string userId, CancellationToken cancellationToken)
        {
            var request = new LogoutUserRequest { UserId = userId };
            var response = await _sender.Send(request, cancellationToken);
            var refreshTokenCookieName = _configuration["Jwt:refreshTokenCookieName"];
            if (refreshTokenCookieName != null) Response.Cookies.Delete(refreshTokenCookieName);

            return Ok(new ApiSuccessResult<LogoutUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(LogoutAsync)}",
                Content = response
            });
        }
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult<ApiSuccessResult<ConfirmEmailResult>>> ConfirmEmailAsync(
        [FromQuery] string email,
        [FromQuery] string code,
        CancellationToken cancellationToken
        )
        {
            var request = new ConfirmEmailRequest { Email = email, Code = code };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<ConfirmEmailResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(ConfirmEmailAsync)}",
                Content = response
            });
        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<ApiSuccessResult<GenerateRefreshTokenResult>>> RefreshAccessTokenAsync(CancellationToken cancellationToken)
        {
            var refreshToken = HttpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new ApiException(
                    StatusCodes.Status400BadRequest,
                    "Refresh token has expired, please re-login"
                );
            }
            var request = new GenerateRefreshTokenRequest { RefreshToken = refreshToken };
            var response = await _sender.Send(request, cancellationToken);

            if (response == null)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Refresh token has expired, please re-login"
                    );
            }
            var newAccessToken = response.AccessToken;
            var newRefreshToken = response.RefreshToken;

            if (newAccessToken == null || newRefreshToken == null)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Refresh token has expired, please re-login"
                    );
            }
            var refreshTokenCookieName = _configuration["Jwt:refreshTokenCookieName"];
            double expireInMinute;
            if (!double.TryParse(_configuration["Jwt:ExpireInMinute"], out expireInMinute))
            {
                expireInMinute = 15.0;
            }
            response.expires_in_second = (int)(expireInMinute * 60);
            if (refreshTokenCookieName != null)
            {
                // Set cookie HttpOnly for refreshToken
                HttpContext.Response.Cookies.Delete(refreshTokenCookieName);
                HttpContext.Response.Cookies.Append(refreshTokenCookieName, newRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(TokenConsts.ExpiryInDays)
                });

            }
            return Ok(new ApiSuccessResult<GenerateRefreshTokenResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(RefreshAccessTokenAsync)}",
                Content = response
            });
        }
        
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<ApiSuccessResult<ForgotPasswordResult>>> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<ForgotPasswordResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(ForgotPasswordAsync)}",
                Content = response
            });
        }
        [HttpGet("ForgotPasswordConfirmation")]
        public async Task<ActionResult<ApiSuccessResult<ForgotPasswordConfirmationResult>>> ForgotPasswordConfirmationAsync(
       [FromQuery] string email,
       [FromQuery] string code,
       [FromQuery] string tempPassword,
       CancellationToken cancellationToken)
        {
            var request = new ForgotPasswordConfirmationRequest { Email = email, TempPassword = tempPassword, Code = code };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<ForgotPasswordConfirmationResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(ForgotPasswordConfirmationAsync)}",
                Content = response
            });
        }

      
    }
}
