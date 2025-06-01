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
    public class UpdateStatusPaymentResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateStatusPaymentRequest : IRequest<UpdateStatusPaymentResult>
    {
        public string OrderId { get; init; }
        public StatusPayment Status { get; init; }
    }

    public class UpdateStatusPaymentValidator : AbstractValidator<UpdateStatusPaymentRequest>
    {
        public UpdateStatusPaymentValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty();
            RuleFor(x => x.Status)
                .NotEmpty();
        }
    }


    public class UpdateStatusPaymentHandler : IRequestHandler<UpdateStatusPaymentRequest, UpdateStatusPaymentResult>
    {
        private readonly IBaseCommandRepository<Order> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStatusPaymentHandler(
            IBaseCommandRepository<Order> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateStatusPaymentResult> Handle(UpdateStatusPaymentRequest request, CancellationToken cancellationToken)
        {

            var entity = await _repository.GetByIdAsync(request.OrderId, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.OrderId}");
            }

            entity.StatusPayment = request.Status;

            _repository.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateStatusPaymentResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
