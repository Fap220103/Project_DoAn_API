using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Filters;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ProductCategories
{
    public class ProductCategoryController : BaseApiController
    {
        public ProductCategoryController(ISender sender) : base(sender)
        {
        }
        [HttpPost("CreateProductCategory")]
        public async Task<ActionResult<ApiSuccessResult<CreateProductCategoryResult>>> CreateProductCategoryAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateProductCategoryAsync)}",
                Content = response
            });
        }
        [HttpPost("UpdateProductCategory")]
        public async Task<ActionResult<ApiSuccessResult<UpdateProductCategoryResult>>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateProductCategoryAsync)}",
                Content = response
            });
        }

        [HttpDelete("DeleteProductCategory")]
        public async Task<ActionResult<ApiSuccessResult<DeleteProductCategoryResult>>> DeleteProductCategoryAsync(DeleteProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteProductCategoryAsync)}",
                Content = response
            });
        }

        [HttpGet("GetAllProductCategory")]
        public async Task<ActionResult<ApiSuccessResult<GetProductCategoryResult>>> GetAllProductCategoryAsync(CancellationToken cancellationToken)
        {
            var request = new GetProductCategoryRequest();
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetAllProductCategoryAsync)}",
                Content = response
            });
        }
    }
}
