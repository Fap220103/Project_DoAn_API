using Application.Services.CQS.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Settings.Queries
{
    public class GeneralSettingDto
    {
        public string SiteName { get; set; }
        public string Address { get; set; }
        public string Hotline { get; set; }
        public string SupportEmail { get; set; }
    }

    public class GetGeneralSettingRequest : IRequest<GeneralSettingDto> { }

    public class GetGeneralSettingHandler : IRequestHandler<GetGeneralSettingRequest, GeneralSettingDto>
    {
        private readonly IQueryContext _context;

        public GetGeneralSettingHandler(IQueryContext context)
        {
            _context = context;
        }

        public async Task<GeneralSettingDto> Handle(GetGeneralSettingRequest request, CancellationToken cancellationToken)
        {
            var settings = await _context.Setting.ToListAsync(cancellationToken);

            string GetValue(string key) =>
                settings.FirstOrDefault(s => s.Key == key)?.Value ?? "";

            return new GeneralSettingDto
            {
                SiteName = GetValue("SiteName"),
                Address = GetValue("Address"),
                Hotline = GetValue("HotLine"),
                SupportEmail = GetValue("SupportEmail")
            };
        }
    }

}
