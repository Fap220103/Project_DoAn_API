using Application.Features.Colors.Queries;
using Application.Features.Configs.Commands;
using Application.Features.Settings.Commands;
using Application.Features.Settings.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Filters;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Configs
{
    [Authorize(Roles = "Manager")]
    public class SettingController : BaseApiController
    {
        public SettingController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<ActionResult<ApiSuccessResult<UpsertSettingResult>>> UpsertSettingAsync(UpsertSettingRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<UpsertSettingResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(UpsertSettingAsync)}",
                Content = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResult<DeleteSettingResult>>> DeleteSettingAsync(string id, CancellationToken cancellationToken)
        {
            var request = new DeleteSettingRequest
            {
                Id = id
            };
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<DeleteSettingResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(DeleteSettingAsync)}",
                Content = response
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiSuccessResult<GetSettingResult>>> GetSettingAsync([FromQuery] GetSettingRequest request, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(request, cancellationToken);

            return Ok(new ApiSuccessResult<GetSettingResult>
            {
                Code = StatusCodes.Status200OK,
                Message = $"Success executing {nameof(GetSettingAsync)}",
                Content = response
            });
        }
        [AllowAnonymous]
        [HttpGet("GetGeneralSetting")]
        public async Task<IActionResult> GetGeneralSettingAsync()
        {
            var result = await _sender.Send(new GetGeneralSettingRequest());
            return Ok(result);
        }
    }
}
