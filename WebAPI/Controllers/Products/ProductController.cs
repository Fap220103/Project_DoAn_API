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
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ApiSuccessResult<UpdateProductResult>>> UpdateProductAsync([FromForm] UpdateProductRequest request, CancellationToken cancellationToken)
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
        public async Task<ActionResult<ApiSuccessResult<GetProductResult>>> GetProductAsync([FromQuery] GetProductRequest request,CancellationToken cancellationToken)
        {
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
        [HttpGet("GetByCategory")]
        public async Task<ActionResult<ApiSuccessResult<GetProductByCategoryResult>>> GetProductByCategoryAsync([FromQuery] GetProductByCategoryRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetProductByCategoryResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetProductByCategoryAsync)}",
                Content = response
            });
        }
        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var result = await _sender.Send(new ExportProductsToExcelRequest());

            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "products.xlsx");
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Vui lòng chọn file Excel.");

            using var stream = file.OpenReadStream();
            var command = new ImportProductsRequest { ExcelFileStream = stream };

            await _sender.Send(command);
            return Ok("Import thành công!");
        }

    }
}
