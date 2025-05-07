
using Application.Features.Colors.Commands;
using Application.Features.Colors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Colors
{

    public class ColorController : BaseApiController
    {
        public ColorController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateColorResult>>> CreateColorAsync(CreateColorRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateColorResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateColorAsync)}",
                Content = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteColorResult>>> DeleteColorAsync(int id, CancellationToken cancellationToken)
        {
            var request = new DeleteColorRequest { ColorId = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteColorResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteColorAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetColorResult>>> GetColorAsync([FromQuery] GetColorRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetColorResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetColorAsync)}",
                Content = response
            });
        }
    }
}
