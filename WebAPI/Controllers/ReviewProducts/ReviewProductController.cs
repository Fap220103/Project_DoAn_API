using Application.Features.Colors.Queries;
using Application.Features.Configs.Commands;
using Application.Features.Reviews.Commands;
using Application.Features.Reviews.Queries;
using Application.Features.Settings.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ReviewProducts
{
    public class ReviewProductController : BaseApiController
    {
        public ReviewProductController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<AddReviewResult>>> AddReviewAsync(AddReviewRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddReviewResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddReviewAsync)}",
                Content = response
            });
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<ApiSuccessResult<DeleteSettingResult>>> DeleteSettingAsync(string id, CancellationToken cancellationToken)
        //{
        //    var request = new DeleteSettingRequest
        //    {
        //        Id = id
        //    };
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<DeleteSettingResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(DeleteSettingAsync)}",
        //        Content = response
        //    });
        //}

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetReviewResult>>> GetReviewAsync([FromQuery] GetReviewRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetReviewResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetReviewAsync)}",
                Content = response
            });
        }


    }
}
