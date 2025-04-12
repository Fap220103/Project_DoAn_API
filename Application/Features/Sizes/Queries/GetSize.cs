using Application.Services.CQS.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Sizes.Queries
{
    public class GetSizeResult
    {
        public IEnumerable<Size> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetSizeRequest : IRequest<GetSizeResult>
    {
    }

    public class GetSizeHandler : IRequestHandler<GetSizeRequest, GetSizeResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetSizeHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetSizeResult> Handle(GetSizeRequest request, CancellationToken cancellationToken)
        {
            var entities = await _context.Size.ToListAsync(cancellationToken);

            return new GetSizeResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
