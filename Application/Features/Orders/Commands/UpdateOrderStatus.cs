using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProductCategories.Commands
{
    public class UpdateOrderStatusResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateOrderStatusRequest : IRequest<UpdateOrderStatusResult>
    {
        public string OrderId { get; init; }
        public int Status { get; init; }
    }

    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusRequest>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();
            RuleFor(x => x.Status)
                .NotEmpty();
        }
    }


    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusRequest, UpdateOrderStatusResult>
    {
        private readonly IBaseCommandRepository<Order> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusHandler(
            IBaseCommandRepository<Order> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateOrderStatusResult> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.OrderId, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.OrderId}");
            }

            entity.Status = request.Status;

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateOrderStatusResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
