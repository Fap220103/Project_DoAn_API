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

namespace Application.Features.ProductImages.Queries
{


    public class GetImagesByProductIdResult
    {
        public IEnumerable<ProductImage> Data { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class GetImagesByProductIdRequest : IRequest<GetImagesByProductIdResult>
    {
        public string ProductId { get; set; } = null!;
    }

    public class GetImagesByProductIdValidator : AbstractValidator<GetImagesByProductIdRequest>
    {
        public GetImagesByProductIdValidator()
        {
            RuleFor(x => x.ProductId)
               .NotEmpty();
        }
    }


    public class GetImagesByProductIdHandler : IRequestHandler<GetImagesByProductIdRequest, GetImagesByProductIdResult>
    {
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public GetImagesByProductIdHandler(
            IQueryContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetImagesByProductIdResult> Handle(GetImagesByProductIdRequest request, CancellationToken cancellationToken)
        {
            var entities = await _context.ProductImage.Where(x=> x.ProductId == request.ProductId).ToListAsync(cancellationToken);
            if(!entities.Any())
            {
                throw new ApplicationException("No images found for this product");
            }
            return new GetImagesByProductIdResult
            {
                Data = entities,
                Message = "Success"
            };
        }
    }
}
