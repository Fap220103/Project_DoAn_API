using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using Application.Features.ProductImages.Commands;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers.Products
{
    public class ProductController : BaseApiController
    {
        public ProductController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiSuccessResult<CreateProductResult>>> CreateProductAsync([FromForm] CreateProductRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateProductResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateProductAsync)}",
                Content = response
            });
        }
        [HttpPut]
        public async Task<ActionResult<ApiSuccessResult<UpdateProductResult>>> UpdateProductAsync(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpdateProductResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpdateProductAsync)}",
                Content = response
            });
        }

        [HttpDelete]
        public async Task<ActionResult<ApiSuccessResult<DeleteProductResult>>> DeleteProductAsync([FromQuery] string UserId,[FromQuery] string ProductId, CancellationToken cancellationToken)
        {
            var request = new DeleteProductRequest
            {
                UserId = UserId,
                ProductId = ProductId
            };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteProductResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteProductAsync)}", 
                Content = response
            });
        }
      
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetProductResult>>> GetProductAsync(CancellationToken cancellationToken)
        {
            var request = new GetProductRequest();
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductAsync)}",
                Content = response
            });
        }
        [HttpGet("GetById")]
        public async Task<ActionResult<ApiSuccessResult<GetProductByIdResult>>> GetProductByIdAsync([FromQuery] string ProductId,CancellationToken cancellationToken)
        {
            var request = new GetProductByIdRequest { ProductId = ProductId};
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductByIdResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductByIdAsync)}",
                Content = response
            });
        }
      
    }
}
