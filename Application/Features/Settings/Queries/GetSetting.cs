using Application.Common.Models;
using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Colors.Queries
{

    public class GetSettingResult
    {
        public IEnumerable<Setting> Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetSettingRequest : IRequest<GetSettingResult>
    {

    }

    public class GetSettingHandler : IRequestHandler<GetSettingRequest, GetSettingResult>
    {
        private readonly IQueryContext _context;

        public GetSettingHandler(
            IQueryContext context
            )
        {
            _context = context;
        }

        public async Task<GetSettingResult> Handle(GetSettingRequest request, CancellationToken cancellationToken)
        {
            var entities = await _context.Setting.ToListAsync(cancellationToken);

            return new GetSettingResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
