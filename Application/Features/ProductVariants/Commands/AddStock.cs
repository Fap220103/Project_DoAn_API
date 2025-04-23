using Application.Services.CQS.Commands;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductVariants.Commands
{
    public class AddStockResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddStockRequest : IRequest<AddStockResult>
    {
        public string ProductVariantId { get; init; }
        public int Quantity { get; set; } = 10;
    }

    public class AddStockValidator : AbstractValidator<AddStockRequest>
    {
        public AddStockValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .NotEmpty();

        }
    }
    public class AddStockHandler : IRequestHandler<AddStockRequest, AddStockResult>
    {
        private readonly ICommandContext _context;

        public AddStockHandler(ICommandContext context)
        {
            _context = context;
        }

        public async Task<AddStockResult> Handle(AddStockRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Quantity <= 0)
            {
                request.Quantity = 1;
            }

            var variant = await _context.ProductVariant
                .FirstOrDefaultAsync(x => x.Id == request.ProductVariantId, cancellationToken);

            if (variant == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound}");
            }

            variant.Quantity += request.Quantity;
          
            // Đây là dòng lưu vào database
            await _context.SaveChangesAsync(cancellationToken);

            return new AddStockResult
            {
                Id = request.ProductVariantId,
                Message = "Success"
            };
        }
    }
}
