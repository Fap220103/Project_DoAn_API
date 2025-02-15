using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Roles
{
    public class RoleController : BaseApiController
    {
        public RoleController(ISender sender) : base(sender)
        {
        }
        [HttpPost("CreateRole")]
        public async Task<ActionResult<ApiSuccessResult<CreateRoleResult>>> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateRoleResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateRoleAsync)}",
                Content = response
            });
        }
        [HttpDelete("DeleteRole")]
        public async Task<ActionResult<ApiSuccessResult<DeleteRoleResult>>> DeleteRoleAsync(DeleteRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteRoleResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteRoleAsync)}",
                Content = response
            });
        }
        [HttpPost("UpdateRole")]
        public async Task<ActionResult<ApiSuccessResult<UpdateRoleResult>>> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateRoleResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateRoleAsync)}",
                Content = response
            });
        }
        [HttpGet("GetRoles")]
        public async Task<ActionResult<ApiSuccessResult<GetRolesResult>>> GetRolesAsync(
          [FromQuery] int skip,
          [FromQuery] int top,
          [FromQuery] string orderBy,
          [FromQuery] string searchValue,
          CancellationToken cancellationToken)
        {
            int pageNumber = (skip / top) + 1;
            int pageSize = top;

            var orderByParts = orderBy.Split(' ');
            var sortBy = orderByParts[0];
            var sortDirection = orderByParts.Length > 1 ? orderByParts[1].ToLower() : "asc";

            var command = new GetRolesRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                SortBy = sortBy,
                SortDirection = sortDirection,
                searchValue = searchValue,
            };
            var response = await _sender.Send(command, cancellationToken);

            return Ok(new ApiSuccessResult<GetRolesResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetRolesAsync)}",
                Content = response
            });
        }
        [HttpGet("GetRolesByUser")]
        public async Task<ActionResult<ApiSuccessResult<GetRolesByUserResult>>> GetRolesByUserAsync(
           [FromQuery] int skip,
           [FromQuery] int top,
           //[FromQuery] string orderBy,
           [FromQuery] string userId,
           CancellationToken cancellationToken)
        {
            int pageNumber = (skip / top) + 1;
            int pageSize = top;

            var command = new GetRolesByUserRequest { pageNumber = pageNumber, pageSize = pageSize, UserId = userId };
            var response = await _sender.Send(command, cancellationToken); ;

            return Ok(new ApiSuccessResult<GetRolesByUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetRolesAsync)}",
                Content = response
            });
        }
    }
}
