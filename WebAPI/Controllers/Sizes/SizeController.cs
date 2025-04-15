using Application.Features.Colors.Commands;
using Application.Features.Colors.Queries;
using Application.Features.Sizes.Commands;
using Application.Features.Sizes.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Sizes
{
    public class SizeController : BaseApiController
    {
        public SizeController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<UpsertSizeResult>>> UpsertSizeAsync(UpsertSizeRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpsertSizeResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpsertSizeAsync)}",
                Content = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteSizeResult>>> DeleteSizeAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteSizeRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteSizeResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteSizeAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetSizeResult>>> GetSizeAsync(CancellationToken cancellationToken)
        {
            var request = new GetSizeRequest();
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetSizeResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetSizeAsync)}",
                Content = response
            });
        }
    }
}
