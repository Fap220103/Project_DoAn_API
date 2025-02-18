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
   
    public class GetColorResult
    {
        public IEnumerable<Color> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetColorRequest : IRequest<GetColorResult>
    {
    }

    public class GetColorHandler : IRequestHandler<GetColorRequest, GetColorResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetColorHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetColorResult> Handle(GetColorRequest request, CancellationToken cancellationToken)
        {
            var entities = await _context.Color.ToListAsync(cancellationToken);

            return new GetColorResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
