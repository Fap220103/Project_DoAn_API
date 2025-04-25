using Application.Services.CQS.Commands;
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
    public class ChangeDefaultAddressResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class ChangeDefaultAddressRequest : IRequest<ChangeDefaultAddressResult>
    {
        public string Id { get; init; }
        public string CustomerId { get; init; }

    }

    public class ChangeDefaultAddressValidator : AbstractValidator<ChangeDefaultAddressRequest>
    {
        public ChangeDefaultAddressValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.CustomerId)
               .NotEmpty();
        }
    }


    public class ChangeDefaultAddressHandler : IRequestHandler<ChangeDefaultAddressRequest, ChangeDefaultAddressResult>
    {
        private readonly IBaseCommandRepository<ShippingAddress> _repository;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeDefaultAddressHandler(
            IBaseCommandRepository<ShippingAddress> repository,
            ICommandContext context,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChangeDefaultAddressResult> Handle(ChangeDefaultAddressRequest request, CancellationToken cancellationToken)
        {
            var entityDefault = _context.ShippingAddress.FirstOrDefault(x => x.IsDefault && x.UserId == request.CustomerId);
            if (entityDefault != null)
            {
                entityDefault.IsDefault = false;
                _context.ShippingAddress.Update(entityDefault);
                _context.SaveChangesAsync();
            }
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if(entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }
            entity.IsDefault = true;
          
            await _unitOfWork.SaveAsync(cancellationToken);

            return new ChangeDefaultAddressResult
            {
                Id = request.Id,
                Message = "Success"
            };
        }
    }
}
