using Application.Common.Models;
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

namespace Application.Features.ProductVariants.Queries
{
    public class GetCZVariantResult
    {
        public List<Color> Color { get; init; }
        public List<Size> Size { get; init; } 
        public string Message { get; init; } = null!;
    }

    public class GetCZVariantRequest : IRequest<GetCZVariantResult>
    {
        public string ProductId { get; set; }
    }

    public class GetCZVariantHandler : IRequestHandler<GetCZVariantRequest, GetCZVariantResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetCZVariantHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetCZVariantResult> Handle(GetCZVariantRequest request, CancellationToken cancellationToken)
        {
            var colors = await _context.ProductVariant
                                         .Include(pv => pv.Color)
                                         .Where(pv => pv.ProductId == request.ProductId && pv.Quantity > 0)
                                         .Select(pv => new Color{
                                            Id = pv.ColorId,
                                            Name = pv.Color.Name,
                                            HexCode = pv.Color.HexCode,
                                         })
                                         .Distinct()
                                         .ToListAsync();

            var sizes = await _context.ProductVariant
                                    .Include(pv => pv.Size)
                                    .Where(pv => pv.ProductId == request.ProductId && pv.Quantity > 0)
                                    .Select(pv => new Size {
                                       Id= pv.SizeId,
                                       Name = pv.Size.Name
                                    })
                                    .Distinct()
                                    .ToListAsync();

            return new GetCZVariantResult
            {
                Color = colors,
                Size = sizes,
                Message = "Success"
            };

        }

    }
}
