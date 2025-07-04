﻿using Application.Services.CQS.Queries;
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

namespace Application.Features.ProductCategories.Commands
{
    public class DeleteProductCategoryResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteProductCategoryRequest : IRequest<DeleteProductCategoryResult>
    {
        public string UserId { get; init; } = null!;
        public string ProductCategoryId { get; init; } = null!;
    }

    public class DeleteProductCategoryValidator : AbstractValidator<DeleteProductCategoryRequest>
    {
        public DeleteProductCategoryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.ProductCategoryId)
                .NotEmpty();
        }
    }


    public class DeleteProductCategoryHandler : IRequestHandler<DeleteProductCategoryRequest, DeleteProductCategoryResult>
    {
        private readonly IBaseCommandRepository<ProductCategory> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCategoryHandler(
            IBaseCommandRepository<ProductCategory> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProductCategoryResult> Handle(DeleteProductCategoryRequest request, CancellationToken cancellationToken = default)
        {
            var query = _repository.GetQuery();

            query = query
                .ApplyIsDeletedFilter()
                .Where(x => x.Id == request.ProductCategoryId);

            var entity = await query.SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ProductCategoryId}");
            }

            entity.Delete(request.UserId);

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteProductCategoryResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
