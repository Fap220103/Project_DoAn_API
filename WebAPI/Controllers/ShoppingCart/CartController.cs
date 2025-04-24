using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.ShoppingCarts.Commands;
using Application.Features.ShoppingCarts.Queries;
using Domain.Constants;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ShoppingCart
{
    public class CartController : BaseApiController
    {
        public CartController(ISender sender) : base(sender)
        {
        }
        //[HttpPost("AddItemToCart")]
        //public async Task<ActionResult<ApiSuccessResult<AddItemToCartResult>>> AddItemAsync(
        //    [FromQuery] string userId, 
        //    [FromQuery] int Quantity,
        //    [FromBody] CartItem item, 
        //    CancellationToken cancellationToken)
        //{
        //    var request = new AddItemToCartRequest { userId = userId, Item = item };
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<AddItemToCartResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(AddItemAsync)}",
        //        Content = response
        //    });
        //}
        [HttpPost("SyncCart")]
        public async Task<ActionResult<ApiSuccessResult<AddItemToCartResult>>> AddItemAsync(
            AddItemToCartRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddItemToCartResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddItemAsync)}",
                Content = response
            });
        }

        [HttpPost("UpdateCart")]
        public async Task<ActionResult<ApiSuccessResult<UpdateCartResult>>> UpdateCartAsync([FromQuery] string productId, [FromQuery] int Quantity,[FromQuery] string userId, CancellationToken cancellationToken)
        {
            var request = new UpdateCartRequest { ProductId = productId, Quantity = Quantity, UserId = userId };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateCartResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateCartAsync)}",
                Content = response
            });
        }

        [HttpDelete("DeleteCart")]
        public async Task<ActionResult<ApiSuccessResult<DeleteCartResult>>> DeleteCartAsync(DeleteCartRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteCartResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteCartAsync)}",
                Content = response
            });
        }
        [HttpDelete("DeleteCartById")]
        public async Task<ActionResult<ApiSuccessResult<DeleteCartByIdResult>>> DeleteCartByIdAsync(
            [FromQuery] string ProductId,
            [FromQuery] string UserId,
            CancellationToken cancellationToken)
        {
            var request = new DeleteCartByIdRequest { ProductId = ProductId, UserId = UserId };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteCartByIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteCartByIdAsync)}",
                Content = response
            });
        }

        [HttpGet("GetCart")]
        public async Task<ActionResult<ApiSuccessResult<GetCartResult>>> GetCartAsync([FromQuery] string userId, CancellationToken cancellationToken)
        {
            var request = new GetCartRequest { userId = userId };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetCartResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetCartAsync)}",
                Content = response
            });
        }

    }
}
