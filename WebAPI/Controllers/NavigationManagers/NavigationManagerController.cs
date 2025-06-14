﻿using Application.Features.NavigationManagers.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Exceptions;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.NavigationManagers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NavigationManagerController : BaseApiController
    {
        public NavigationManagerController(ISender sender) : base(sender)
        {
        }
        [HttpGet("GetMainNav")]
        public async Task<ActionResult<ApiSuccessResult<GetMainNavResult>>> GetMainNavAsync(string userId, CancellationToken cancellationToken)
        {
            var command = new GetMainNavRequest { UserId = userId };
            var response = await _sender.Send(command, cancellationToken);

            if (response == null)
            {
                throw new ApiException(
                    StatusCodes.Status401Unauthorized,
                    "Invalid navigation builder"
                    );
            }

            return Ok(new ApiSuccessResult<GetMainNavResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetMainNavAsync)}",
                Content = response
            });
        }
    }
}
