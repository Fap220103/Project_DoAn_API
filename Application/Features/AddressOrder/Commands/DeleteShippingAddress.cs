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

namespace Application.Features.AddressOrder.Commands
{
    public class DeleteShippingAddressResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteShippingAddressRequest : IRequest<DeleteShippingAddressResult>
    {
        public string Id { get; init; } = null!;
    }

    public class DeleteShippingAddressValidator : AbstractValidator<DeleteShippingAddressRequest>
    {
        public DeleteShippingAddressValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }


    public class DeleteShippingAddressHandler : IRequestHandler<DeleteShippingAddressRequest, DeleteShippingAddressResult>
    {
        private readonly IBaseCommandRepository<ShippingAddress> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteShippingAddressHandler(
            IBaseCommandRepository<ShippingAddress> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteShippingAddressResult> Handle(DeleteShippingAddressRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }

            if(entity.IsDefault)
            {
                throw new ApplicationException("Không được phép xóa địa chỉ mặc định");
            }

            _repository.Purge(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteShippingAddressResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
