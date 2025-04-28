using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductVariants.Commands;
using Application.Features.ProductVariants.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ProductVariants
{
    [Authorize(Roles = "Admin,Staff")]
    public class ProductVariantController : BaseApiController
    {
        public ProductVariantController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateProductVariantResult>>> CreateProductVariantAsync(CreateProductVariantRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateProductVariantResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateProductVariantAsync)}",
                Content = response
            });
        }
        [HttpPost("AddStock")]
        public async Task<ActionResult<ApiSuccessResult<AddStockResult>>> AddStockAsync(AddStockRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<AddStockResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(AddStockAsync)}",
                Content = response
            });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteProductVariantResult>>> DeleteProductVariantAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteProductVariantRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteProductVariantResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteProductVariantAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetProductVariantResult>>> GetProductVariantAsync([FromQuery] GetProductVariantRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductVariantResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductVariantAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetCZVariant")]
        public async Task<ActionResult<ApiSuccessResult<GetCZVariantResult>>> GetCZVariantAsync([FromQuery] GetCZVariantRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetCZVariantResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetCZVariantAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetProductVariantId")]
        public async Task<ActionResult<ApiSuccessResult<GetProductVariantIdResult>>> GetProductVariantIdAsync([FromQuery] GetProductVariantIdRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductVariantIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductVariantIdAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetStock")]
        public async Task<ActionResult<ApiSuccessResult<GetStockResult>>> GetStockAsync([FromQuery] GetStockRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetStockResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetStockAsync)}",
                Content = response
            });
        }
    }
}
