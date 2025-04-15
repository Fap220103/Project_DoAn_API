using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Common.Filters;
using WebAPI.Common.Models;

namespace WebAPI.Controllers.Configs
{
    public class SettingController : BaseApiController
    {
        public SettingController(ISender sender) : base(sender)
        {
        }
        //[HttpPost]
        //public async Task<ActionResult<ApiSuccessResult<CreateConfigResult>>> CreateConfigAsync(CreateConfigRequest request, CancellationToken cancellationToken)
        //{
        //    var response = await _sender.Send(request, cancellationToken);

        //    return Ok(new ApiSuccessResult<CreateConfigResult>
        //    {
        //        Code = StatusCodes.Status200OK,
        //        Message = $"Success executing {nameof(CreateConfigAsync)}",
        //        Content = response
        //    });
        //}

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
        public async Task<ActionResult<ApiSuccessResult<DeleteSettingResult>>> DeleteSettingAsync(DeleteSettingRequest request, CancellationToken cancellationToken)
        {
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


    }
}
