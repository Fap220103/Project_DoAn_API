using Application.Features.Discounts.Commands;
using Application.Features.Discounts.Queries;
using Application.Features.ProductCategories.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Discounts
{
    [Authorize(Roles = "Admin")]
    public class DiscountController : BaseApiController
    {
        public DiscountController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateDiscountResult>>> CreateDiscountAsync(CreateDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateDiscountAsync)}",
                Content = response
            });
        }
        [HttpPut]
        public async Task<ActionResult<ApiSuccessResult<UpdateDiscountResult>>> UpdateDiscountAsync(UpdateDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateDiscountAsync)}",
                Content = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteDiscountResult>>> DeleteDiscountAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteDiscountRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteDiscountAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetDiscountResult>>> GetDiscountAsync([FromQuery] GetDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetDiscountAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetDiscountAdmin")]
        public async Task<ActionResult<ApiSuccessResult<GetDiscountAdminResult>>> GetDiscountAdminAsync([FromQuery] GetDiscountAdminRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetDiscountAdminResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetDiscountAdminAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetUserDiscount")]
        public async Task<ActionResult<ApiSuccessResult<GetUserDiscountResult>>> GetUserDiscountAsync([FromQuery] GetUserDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetUserDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetUserDiscountAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetStatusUserDiscount")]
        public async Task<ActionResult<ApiSuccessResult<GetStatusUserDiscountResult>>> GetStatusUserDiscountAsync([FromQuery] GetStatusUserDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetStatusUserDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetStatusUserDiscountAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpPost("AddUserDiscount")]
        public async Task<ActionResult<ApiSuccessResult<AddUserDiscountResult>>> AddUserDiscountAsync(AddUserDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddUserDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddUserDiscountAsync)}",
                Content = response
            });
        }
    }
}
