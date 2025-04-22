using Application.Services.CQS.Commands;
using Application.Services.CQS.Queries;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Inventories.Commands
{
    public class CreateInventoryResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class CreateInventoryRequest : IRequest<CreateInventoryResult>
    {
        public string ProductVariantId { get; set; } = null!;
        public int Quantity { get; set; } = 10;
    }

    public class CreateInventoryValidator : AbstractValidator<CreateInventoryRequest>
    {
        public CreateInventoryValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .NotEmpty();
        }
    }
    public class CreateInventoryHandler : IRequestHandler<CreateInventoryRequest, CreateInventoryResult>
    {
        private readonly ICommandContext _context;

        public CreateInventoryHandler(ICommandContext context)
        {
            _context = context;
        }

        public async Task<CreateInventoryResult> Handle(CreateInventoryRequest request, CancellationToken cancellationToken = default)
        {
            var variant = await _context.ProductVariant
                .FirstOrDefaultAsync(x => x.Id == request.ProductVariantId, cancellationToken);

            if (variant == null)
            {
                throw new Exception($"ProductVariant with ID {request.ProductVariantId} not found.");
            }

            var existingInventory = await _context.Inventory
                .FirstOrDefaultAsync(i => i.ProductVariantId == request.ProductVariantId, cancellationToken);

            if (existingInventory != null)
            {
                existingInventory.Quantity = request.Quantity;
                existingInventory.LastUpdated = DateTime.UtcNow;
            }
            else
            {
                var newInventory = new Inventory
                {
                    ProductVariantId = request.ProductVariantId,
                    Quantity = request.Quantity,
                    LastUpdated = DateTime.UtcNow
                };

                await _context.Inventory.AddAsync(newInventory, cancellationToken);
            }

            // Đây là dòng lưu vào database
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateInventoryResult
            {
                Id = request.ProductVariantId,
                Message = "Success"
            };
        }
    }

}
