using Application.Features.Colors.Queries;
using Application.Features.Configs.Commands;
using Application.Features.NewsManager.Commands;
using Application.Features.NewsManager.Queries;
using Application.Features.Settings.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.News
{

    public class NewsController : BaseApiController
    {
        public NewsController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<AddNewsResult>>> AddNewsAsync([FromForm] AddNewsRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddNewsResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddNewsAsync)}",
                Content = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteNewsResult>>> DeleteNewsAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteNewsRequest
            {
                NewId = id
            };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteNewsResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteNewsAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetNewsResult>>> GetNewsAsync([FromQuery] GetNewsRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetNewsResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetNewsAsync)}",
                Content = response
            });
        }
        [HttpGet("GetNewsById")]
        public async Task<ActionResult<ApiSuccessResult<GetNewsByIdResult>>> GetNewsByIdAsync([FromQuery] GetNewsByIdRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetNewsByIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetNewsByIdAsync)}",
                Content = response
            });
        }
    }
}
