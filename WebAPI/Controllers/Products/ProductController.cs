using Application.Features.ProductCategories.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Products
{
    public class ProductController : BaseApiController
    {
        public ProductController(ISender sender) : base(sender)
        {
        }
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ApiSuccessResult<CreateProductResult>>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateProductResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateProductAsync)}",
                Content = response
            });
        }
    }
}
