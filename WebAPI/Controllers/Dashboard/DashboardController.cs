using Application.Features.Dashboard.Queries;
using Application.Features.Discounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Dashboard
{
    public class DashboardController : BaseApiController
    {
        public DashboardController(ISender sender) : base(sender)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetInfoDashboardResult>>> GetInfoDashboardAsync(CancellationToken cancellationToken)
        {
            var request = new GetInfoDashboardRequest();
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetInfoDashboardResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetInfoDashboardAsync)}",
                Content = response
            });
        }
    }
}
