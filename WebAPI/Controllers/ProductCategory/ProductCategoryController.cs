using Application.Features.ProductCategories.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.ProductCategory
{
    public class ProductCategoryController : BaseApiController
    {
        public ProductCategoryController(ISender sender) : base(sender)
        {
        }
        [HttpPost("CreateProductCategory")]
        public async Task<ActionResult<ApiSuccessResult<CreateProductCategoryResult>>> CreateCustomerAsync(CreateProductCategoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateProductCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateCustomerAsync)}",
                Content = response
            });
        }
    }
}
