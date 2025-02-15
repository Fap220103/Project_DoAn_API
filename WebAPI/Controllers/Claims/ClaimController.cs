using Application.Features.Accounts.Commands;
using Application.Features.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Claims
{
    public class ClaimController : BaseApiController
    {
        public ClaimController(ISender sender) : base(sender)
        {         
        }
        [HttpPost("AddClaimsToRole")]
        public async Task<ActionResult<ApiSuccessResult<AddClaimsToRoleResult>>> AddClaimsToRoleAsync(AddClaimsToRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddClaimsToRoleResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddClaimsToRoleAsync)}",
                Content = response
            });
        }
        [HttpDelete("DeleteClaimsFromRole")]
        public async Task<ActionResult<ApiSuccessResult<DeleteClaimsFromRoleResult>>> DeleteClaimsFromRoleAsync(DeleteClaimsFromRoleRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteClaimsFromRoleResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteClaimsFromRoleAsync)}",
                Content = response
            });
        }
        [HttpGet("GetClaimsByUser")]
        public async Task<ActionResult<ApiSuccessResult<GetClaimsByUserResult>>> GetClaimsByUserAsync(
           [FromQuery] string userId,
           [FromQuery] int skip = 1,
           [FromQuery] int top = 10,
           [FromQuery] string? orderBy = "value",
           [FromQuery] string? searchValue = "",     
           CancellationToken cancellationToken = default)
        {
            
            int pageNumber = (skip / top) + 1;
            int pageSize = top;

            var orderByParts = orderBy.Split(' ');
            var sortBy = orderByParts[0];
            var sortDirection = orderByParts.Length > 1 ? orderByParts[1].ToLower() : "asc";

            var command = new GetClaimsByUserRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                SortBy = sortBy,
                SortDirection = sortDirection,
                searchValue = searchValue,
                UserId = userId
            };
            var response = await _sender.Send(command, cancellationToken);

            return Ok(new ApiSuccessResult<GetClaimsByUserResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetClaimsByUserAsync)}",
                Content = response
            });
        }
        [HttpGet("GetClaims")]
        public async Task<ActionResult<ApiSuccessResult<GetClaimsResult>>> GetClaimsAsync(
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

            var command = new GetClaimsRequest
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                SortBy = sortBy,
                SortDirection = sortDirection,
                searchValue = searchValue
            };
            var response = await _sender.Send(command, cancellationToken);

            return Ok(new ApiSuccessResult<GetClaimsResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetClaimsByUserAsync)}",
                Content = response
            });
        }
        [HttpGet("GetClaimsByRole")]
        public async Task<ActionResult<ApiSuccessResult<GetClaimsByRoleResult>>> GetClaimsByRoleAsync(
            [FromQuery] int skip,
            [FromQuery] int top,
            [FromQuery] string role,
            CancellationToken cancellationToken)
        {
            int pageNumber = (skip / top) + 1;
            int pageSize = top;

            var command = new GetClaimsByRoleRequest { pageNumber = pageNumber, pageSize = pageSize, Role = role };
            var response = await _sender.Send(command, cancellationToken);

            return Ok(new ApiSuccessResult<GetClaimsByRoleResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetClaimsByRoleAsync)}",
                Content = response
            });
        }
    }
}
