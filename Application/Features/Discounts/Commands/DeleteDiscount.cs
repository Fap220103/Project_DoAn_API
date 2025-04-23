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

namespace Application.Features.Discounts.Commands
{
    public class DeleteDiscountResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteDiscountRequest : IRequest<DeleteDiscountResult>
    {
        public string Id { get; init; } = null!;
    }

    public class DeleteDiscountValidator : AbstractValidator<DeleteDiscountRequest>
    {
        public DeleteDiscountValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteDiscountHandler : IRequestHandler<DeleteDiscountRequest, DeleteDiscountResult>
    {
        private readonly IBaseCommandRepository<Discount> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDiscountHandler(
            IBaseCommandRepository<Discount> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteDiscountResult> Handle(DeleteDiscountRequest request, CancellationToken cancellationToken = default)
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

            return new DeleteDiscountResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
