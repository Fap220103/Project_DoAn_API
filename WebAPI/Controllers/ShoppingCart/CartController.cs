using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Application.Features.ShoppingCarts.Commands;
using Application.Features.ShoppingCarts.Queries;
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
        [HttpPost("AddItemToCart")]
        public async Task<ActionResult<ApiSuccessResult<AddItemToCartResult>>> AddItemToCartAsync([FromQuery] string productId, [FromQuery] int Quantity, CancellationToken cancellationToken)
        { 
            var request = new AddItemToCartRequest { ProductId = productId, Quantity = Quantity };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddItemToCartResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddItemToCartAsync)}",
                Content = response
            });
        }

        [HttpPost("UpdateCart")]
        public async Task<ActionResult<ApiSuccessResult<UpdateCartResult>>> UpdateCartAsync([FromQuery] string productId, [FromQuery] int Quantity, CancellationToken cancellationToken)
        {
            var request = new UpdateCartRequest { ProductId = productId, Quantity = Quantity };
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
        public async Task<ActionResult<ApiSuccessResult<DeleteCartByIdResult>>> DeleteCartByIdAsync([FromQuery] string ProductId, CancellationToken cancellationToken)
        {
            var request = new DeleteCartByIdRequest { ProductId = ProductId };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteCartByIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteCartByIdAsync)}",
                Content = response
            });
        }

        [HttpGet("GetCart")]
        public async Task<ActionResult<ApiSuccessResult<GetCartResult>>> GetCartAsync(CancellationToken cancellationToken)
        {
            var request = new GetCartRequest();
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
