
using Application.Features.Sizes.Commands;
using Application.Features.Sizes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<ApiSuccessResult<CreateSizeResult>>> CreateSizeAsync(CreateSizeRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateSizeResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateSizeAsync)}",
                Content = response
            });
        }
        //[HttpPost("UpdateSize")]
        //public async Task<ActionResult<ApiSuccessResult<UpdateSizeResult>>> UpdateSizeAsync(UpdateSizeRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<UpdateSizeResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(UpdateSizeAsync)}",
        //        Content = response
        //    });
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteSizeResult>>> DeleteSizeAsync(int id, CancellationToken cancellationToken)
        {
            var request = new DeleteSizeRequest
            {
                SizeId = id
            };
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
