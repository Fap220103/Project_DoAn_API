using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Queries;
using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.UserProfiles
{
    public class UserProfileController : BaseApiController
    {
        public UserProfileController(ISender sender) : base(sender)
        {
        }
       
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetUsersResult>>> GetUsersAsync([FromQuery] GetUsersRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetUsersResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetUsersAsync)}",
                Content = response
            });
        }
        [HttpGet("GetProfile")]
        public async Task<ActionResult<ApiSuccessResult<GetUserByIdResult>>> GetUserByIdAsync([FromQuery] GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetUserByIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetUserByIdAsync)}",
                Content = response
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateUserResult>>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateUserAsync)}",
                Content = response
            });
        }

        [HttpPut]
        public async Task<ActionResult<ApiSuccessResult<UpdateUserResult>>> UpdateUserAsync([FromForm] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateUserAsync)}",
                Content = response
            });
        }
        [HttpPut("UpdateProfile")]
        public async Task<ActionResult<ApiSuccessResult<UpdateProfileResult>>> UpdateProfileAsync([FromForm] UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateProfileResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateProfileAsync)}",
                Content = response
            });
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteUserResult>>> DeleteUserAsync(string userId, CancellationToken cancellationToken)
        {
            var request = new DeleteUserRequest { UserId = userId };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteUserAsync)}",
                Content = response
            });
        }
    }
}
