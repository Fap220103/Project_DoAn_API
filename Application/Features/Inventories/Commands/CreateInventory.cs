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
        private readonly IBaseCommandRepository<ProductVariant> _repository;
        private readonly IQueryContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CreateInventoryHandler(
            IBaseCommandRepository<ProductVariant> repository,
            IQueryContext context,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateInventoryResult> Handle(CreateInventoryRequest request, CancellationToken cancellationToken = default)
        {
            var variant = await _repository.GetByIdAsync(request.ProductVariantId, cancellationToken);

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

            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateInventoryResult
            {
                Id = request.ProductVariantId,
                Message = "Success"
            };
        }

    }
}
