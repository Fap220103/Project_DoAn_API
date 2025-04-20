using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductVariants.Commands;
using Application.Features.ProductVariants.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ProductVariants
{
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
     
    }
}
