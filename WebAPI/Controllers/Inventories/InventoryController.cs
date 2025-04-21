using Application.Features.Inventories.Commands;
using Application.Features.Inventories.Queries;
using Application.Features.ProductVariants.Commands;
using Application.Features.ProductVariants.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Inventories
{
    public class InventoryController : BaseApiController
    {
        public InventoryController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateInventoryResult>>> CreateInventoryAsync(CreateInventoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateInventoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateInventoryAsync)}",
                Content = response
            });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteInventoryResult>>> DeleteInventoryAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteInventoryRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteInventoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteInventoryAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetInventoryResult>>> GetInventoryAsync([FromQuery] GetInventoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetInventoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetInventoryAsync)}",
                Content = response
            });
        }
    }
}
