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
        public PagedList<Color> Data { get; set; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetSettingRequest : IRequest<GetSettingResult>
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? Order { get; set; }
        public string? Search { get; set; }
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
            var query = _context.Color.AsQueryable();

           

            return new GetSettingResult
            {
                Data = pagedList,
                Message = "Success"
            };
        }
    }
}
