
using Application.Features.Colors.Queries;
using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Colors
{
    [Authorize(Roles = "Admin")]
    public class ColorController : BaseApiController
    {
        public ColorController(ISender sender) : base(sender)
        {
        }
        //[HttpPost]
        //public async Task<ActionResult<ApiSuccessResult<UpsertColorResult>>> UpsertColorAsync(UpsertColorRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<UpsertColorResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(UpsertColorAsync)}",
        //        Content = response
        //    });
        //}

        //[HttpDelete]
        //public async Task<ActionResult<ApiSuccessResult<DeleteColorResult>>> DeleteColorAsync([FromQuery] DeleteColorRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<DeleteColorResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(DeleteColorAsync)}",
        //        Content = response
        //    });
        //}

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
