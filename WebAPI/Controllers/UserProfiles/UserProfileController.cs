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

        [HttpPost("UpdateUser")]
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
