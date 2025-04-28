using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using Application.Features.ProductCategories.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Orders
{
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
        [HttpPut("UpdateOrderStatus")]
        public async Task<ActionResult<ApiSuccessResult<UpdateOrderStatusResult>>> UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateOrderStatusResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateOrderStatusAsync)}",
                Content = response
            });
        }

        [HttpPost("CancelOrder")]
        public async Task<ActionResult<ApiSuccessResult<CancelOrderResult>>> CancelOrderAsync(CancelOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CancelOrderResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CancelOrderAsync)}",
                Content = response
            });
        }
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
