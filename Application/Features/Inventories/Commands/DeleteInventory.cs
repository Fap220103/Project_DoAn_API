using Application.Services.CQS.Queries;
using Application.Services.Repositories;
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

namespace Application.Features.Inventories.Commands
{
    public class DeleteInventoryResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteInventoryRequest : IRequest<DeleteInventoryResult>
    {
        public string Id { get; init; } = null!;
    }

    public class DeleteInventoryValidator : AbstractValidator<DeleteInventoryRequest>
    {
        public DeleteInventoryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteInventoryHandler : IRequestHandler<DeleteInventoryRequest, DeleteInventoryResult>
    {
        private readonly IQueryContext _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInventoryHandler(
            IQueryContext repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteInventoryResult> Handle(DeleteInventoryRequest request, CancellationToken cancellationToken = default)
        {
            var query = _repository.Inventory.AsQueryable();

            query = query.Where(x => x.ProductVariantId == request.Id);

            var entity = await query.SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            _repository.Inventory.Remove(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteInventoryResult
            {
                Id = entity.ProductVariantId,
                Message = "Success"
            };
        }
    }
}
