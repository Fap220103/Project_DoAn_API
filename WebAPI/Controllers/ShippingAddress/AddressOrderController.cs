using Application.Features.AddressOrder.Commands;
using Application.Features.AddressOrder.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ShippingAddress
{
    public class AddressOrderController : BaseApiController
    {
        public AddressOrderController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<AddShippingAddressResult>>> AddShippingAddressAsync(AddShippingAddressRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddShippingAddressResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddShippingAddressAsync)}",
                Content = response
            });
        }
        [HttpPut]
        public async Task<ActionResult<ApiSuccessResult<ChangeDefaultAddressResult>>> ChangeDefaultAddressAsync(ChangeDefaultAddressRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<ChangeDefaultAddressResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(ChangeDefaultAddressAsync)}",
                Content = response
            });
        }
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<ApiSuccessResult<UpdateShippingAddressResult>>> UpdateShippingAddressAsync(UpdateShippingAddressRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateShippingAddressResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateShippingAddressAsync)}",
                Content = response
            });
        }
        //[HttpPost("ChangeDefaultAddress")]
        //public async Task<ActionResult<ApiSuccessResult<ChangeDefaultResult>>> ChangeDefaultAsync([FromBody] ChangeDefaultRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<ChangeDefaultResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(ChangeDefaultAsync)}",
        //        Content = response
        //    });
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteShippingAddressResult>>> DeleteShippingAddressAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteShippingAddressRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteShippingAddressResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteShippingAddressAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetShippingAddressResult>>> GetShippingAddressAsync([FromQuery] GetShippingAddressRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetShippingAddressResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetShippingAddressAsync)}",
                Content = response
            });
        }

        [HttpGet("GetAddressDefault")]
        public async Task<ActionResult<ApiSuccessResult<GetAddressDefaultResult>>> GetAddressDefaultAsync([FromQuery] GetAddressDefaultRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetAddressDefaultResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetShippingAddressAsync)}",
                Content = response
            });
        }
    }
}
