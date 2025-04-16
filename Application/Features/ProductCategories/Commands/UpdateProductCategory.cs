using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Commands
{
    public class UpdateProductCategoryResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateProductCategoryRequest : IRequest<UpdateProductCategoryResult>
    {
        public string Id { get; init; } = null!;
        public string Title { get; init; } 
        public string? Alias { get; init; } 
        public string? Description { get; init; }
        public int Level { get; init; }
        public string? ParentId { get; set; }
        public bool IsActive { get; init; }
    }

    public class UpdateProductCategoryValidator : AbstractValidator<UpdateProductCategoryRequest>
    {
        public UpdateProductCategoryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.Title)
                .NotEmpty();         
        }
    }


    public class UpdateProductCategoryHandler : IRequestHandler<UpdateProductCategoryRequest, UpdateProductCategoryResult>
    {
        private readonly IBaseCommandRepository<ProductCategory> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCategoryHandler(
            IBaseCommandRepository<ProductCategory> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProductCategoryResult> Handle(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            //if (string.IsNullOrEmpty(request.ParentId))
            //{
            //    request.ParentId = entity.ParentId.ToString();
            //}

            entity.Update(
                    request.Title,
                    request.Alias,
                    request.Description,
                    request.Level,
                    request.ParentId,
                    request.IsActive
                );

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateProductCategoryResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
