using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Orders
{
    [Authorize(Roles = "Admin,Staff")]
    public class OrderController : BaseApiController
    {
        public OrderController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateOrderResult>>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateOrderResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateOrderAsync)}",
                Content = response
            });
        }
        //[HttpPost("UpdateProductCategory")]
        //public async Task<ActionResult<ApiSuccessResult<UpdateProductCategoryResult>>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<UpdateProductCategoryResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(UpdateProductCategoryAsync)}",
        //        Content = response
        //    });
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ApiSuccessResult<DeleteProductCategoryResult>>> DeleteProductCategoryAsync(string id, CancellationToken cancellationToken)
        //{
        //    var request = new DeleteProductCategoryRequest { Id = id };
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<DeleteProductCategoryResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(DeleteProductCategoryAsync)}",
        //        Content = response
        //    });
        //}
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetOrderResult>>> GetOrderAsync([FromQuery] GetOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetOrderResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetOrderAsync)}",
                Content = response
            });
        }
        [HttpGet("GetOrderById")]
        public async Task<ActionResult<ApiSuccessResult<GetOrderByIdResult>>> GetOrderByIdAsync([FromQuery] GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetOrderByIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetOrderByIdAsync)}",
                Content = response
            });
        }
    }
}
