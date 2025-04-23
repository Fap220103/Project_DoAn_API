using Application.Features.Discounts.Commands;
using Application.Features.Discounts.Queries;
using Application.Features.ProductCategories.Commands;
using Application.Features.ProductCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Discounts
{
    public class DiscountController : BaseApiController
    {
        public DiscountController(ISender sender) : base(sender)
        {
        }
        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<CreateDiscountResult>>> CreateDiscountAsync(CreateDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<CreateDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(CreateDiscountAsync)}",
                Content = response
            });
        }
        //[HttpPost("UpdateProductCategory")]
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteDiscountResult>>> DeleteDiscountAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteDiscountRequest { Id = id };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteDiscountAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetDiscountResult>>> GetDiscountAsync([FromQuery] GetDiscountRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetDiscountResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetDiscountAsync)}",
                Content = response
            });
        }
       
    }
}
