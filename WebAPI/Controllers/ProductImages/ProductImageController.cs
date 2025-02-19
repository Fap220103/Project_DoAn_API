using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
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
        //[HttpDelete("DeleteImage")]
        //public async Task<ActionResult<ApiSuccessResult<UpdateProductCategoryResult>>> UpdateProductCategoryAsync(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<UpdateProductCategoryResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(UpdateProductCategoryAsync)}",
        //        Content = response
        //    });
        //}

        //[HttpPost("ChangeDefault")]
        //public async Task<ActionResult<ApiSuccessResult<DeleteProductCategoryResult>>> DeleteProductCategoryAsync(DeleteProductCategoryRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<DeleteProductCategoryResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(DeleteProductCategoryAsync)}",
        //        Content = response
        //    });
        //}

        [HttpGet("GetImages")]
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
