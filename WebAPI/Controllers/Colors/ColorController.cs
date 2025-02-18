using Application.Features.Colors.Commands;
using Application.Features.Colors.Queries;
using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Colors
{
    public class ColorController : BaseApiController
    {
        public ColorController(ISender sender) : base(sender)
        {
        }
        [HttpPost("CreateColor")]
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
        [HttpPost("UpdateColor")]
        public async Task<ActionResult<ApiSuccessResult<UpdateColorResult>>> UpdateColorAsync(UpdateColorRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateColorResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateColorAsync)}",
                Content = response
            });
        }

        [HttpDelete("DeleteColor")]
        public async Task<ActionResult<ApiSuccessResult<DeleteColorResult>>> DeleteColorAsync(DeleteColorRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteColorResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteColorAsync)}",
                Content = response
            });
        }

        [HttpGet("GetColor")]
        public async Task<ActionResult<ApiSuccessResult<GetColorResult>>> GetColorAsync(CancellationToken cancellationToken)
        {
            var request = new GetColorRequest();
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
