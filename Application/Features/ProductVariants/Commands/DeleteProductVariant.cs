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

namespace Application.Features.ProductVariants.Commands
{
    public class DeleteProductVariantResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteProductVariantRequest : IRequest<DeleteProductVariantResult>
    {
        public string Id { get; init; } = null!;
    }

    public class DeleteProductVariantValidator : AbstractValidator<DeleteProductVariantRequest>
    {
        public DeleteProductVariantValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteProductVariantHandler : IRequestHandler<DeleteProductVariantRequest, DeleteProductVariantResult>
    {
        private readonly IBaseCommandRepository<ProductVariant> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductVariantHandler(
            IBaseCommandRepository<ProductVariant> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductVariantResult> Handle(DeleteProductVariantRequest request, CancellationToken cancellationToken = default)
        {
            var query = _repository.GetQuery();

            query = query.Where(x => x.Id == request.Id);

            var entity = await query.SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            _repository.Purge(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteProductVariantResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
