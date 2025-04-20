using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Filters;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ProductCategories
{
    //[Authorize(Roles = "Admin")]
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteProductCategoryResult>>> DeleteProductCategoryAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteProductCategoryRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteProductCategoryAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetProductCategoryResult>>> GetProductCategoryAsync([FromQuery] GetProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductCategoryAsync)}",
                Content = response
            });
        }
        [HttpGet("gettoken")]
        public async Task<ActionResult<ApiSuccessResult<GetProductCategoryNameResult>>> GetProductCategoryNameAsync(GetProductCategoryNameRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductCategoryNameResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductCategoryNameAsync)}",
                Content = response
            });
        }
    }
}
