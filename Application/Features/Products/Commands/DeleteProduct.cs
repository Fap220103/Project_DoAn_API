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

namespace Application.Features.Products.Commands
{
    public class DeleteProductResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteProductRequest : IRequest<DeleteProductResult>
    {
        public string UserId { get; init; } = null!;
        public string ProductId { get; init; } = null!;
    }

    public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ProductId)
                .NotEmpty();
        }
    }


    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, DeleteProductResult>
    {
        private readonly IBaseCommandRepository<Product> _repoProduct;
        private readonly IBaseCommandRepository<ProductImage> _repoProductImage;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductHandler(
            IBaseCommandRepository<Product> repoProduct,
            IBaseCommandRepository<ProductImage> repoProductImage,
            IUnitOfWork unitOfWork
            )
        {
            _repoProduct = repoProduct;
            _repoProductImage = repoProductImage;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductResult> Handle(DeleteProductRequest request, CancellationToken cancellationToken = default)
        {
            var query = _repoProduct.GetQuery();

            query = query
                .ApplyIsDeletedFilter()
                .Where(x => x.Id == request.ProductId);

            var entity = await query.SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ProductId}");
            }

            entity.Delete(request.UserId);

            _repoProduct.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteProductResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
