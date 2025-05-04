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
    }
}
