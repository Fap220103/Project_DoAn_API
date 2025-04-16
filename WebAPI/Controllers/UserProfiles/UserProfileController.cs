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
    }
}
