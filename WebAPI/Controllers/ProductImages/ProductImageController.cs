using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductImages.Commands;
using Application.Features.ProductImages.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ProductImages
{
    public class ProductImageController : BaseApiController
    {
        public ProductImageController(ISender sender) : base(sender)
        {
        }
        [HttpPost("AddImage")]
        public async Task<ActionResult<ApiSuccessResult<AddImageResult>>> AddImageAsync([FromForm] AddImageRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddImageResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddImageAsync)}",
                Content = response
            });
        }
        [HttpDelete("DeleteImage")]
        public async Task<ActionResult<ApiSuccessResult<DeleteImageResult>>> DeleteImageAsync([FromQuery] string ImageId, CancellationToken cancellationToken)
        {
            var request = new DeleteImageRequest
            {
                ImageId = ImageId
            };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteImageResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteImageAsync)}",
                Content = response
            });
        }

        [HttpPost("ChangeDefault")]
        public async Task<ActionResult<ApiSuccessResult<ChangeDefaultResult>>> ChangeDefaultAsync(
            [FromQuery] string ImageId, 
            [FromQuery] string IdDefault, 
            CancellationToken cancellationToken)
        {
            var request = new ChangeDefaultRequest
            {
                ImageId = ImageId,
                IdDefault = IdDefault
            };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<ChangeDefaultResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(ChangeDefaultAsync)}",
                Content = response
            });
        }

        [HttpGet("GetImagesByProductId")]
        public async Task<ActionResult<ApiSuccessResult<GetImagesByProductIdResult>>> GetImagesByProductIdAsync([FromQuery] string ProductId,CancellationToken cancellationToken)
        {
            var request = new GetImagesByProductIdRequest 
            {
                ProductId = ProductId
            };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetImagesByProductIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetImagesByProductIdAsync)}",
                Content = response
            });
        }
    }
}
