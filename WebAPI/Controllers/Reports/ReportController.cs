using Application.Features.ProductVariants.Queries;
using Application.Features.Report.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Reports
{

    public class ReportController : BaseApiController
    {
        public ReportController(ISender sender) : base(sender)
        {
        }
        [HttpGet("GetRevenue")]
        public async Task<ActionResult<ApiSuccessResult<GetRevenueResult>>> GetRevenueAsync([FromQuery] GetRevenueRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetRevenueResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetRevenueAsync)}",
                Content = response
            });
        }
        [HttpGet("BestSeller")]
        public async Task<ActionResult<ApiSuccessResult<BestSellerResult>>> BestSellerAsync(CancellationToken cancellationToken)
        {
            var request = new BestSellerRequest();
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<BestSellerResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(BestSellerAsync)}",
                Content = response
            });
        }
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetReportResult>>> GetReportAsync([FromQuery] GetReportRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetReportResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetReportAsync)}",
                Content = response
            });
        }
        [HttpGet("BadProduct")]
        public async Task<ActionResult<ApiSuccessResult<GetBadProductResult>>> GetBadProductAsync(CancellationToken cancellationToken)
        {
            var request = new GetBadProductRequest();
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetBadProductResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetBadProductAsync)}",
                Content = response
            });
        }
    }
}
