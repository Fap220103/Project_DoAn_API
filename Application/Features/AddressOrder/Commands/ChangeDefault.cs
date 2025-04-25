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
    public class ChangeDefaultResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class ChangeDefaultRequest : IRequest<ChangeDefaultResult>
    {
        public string Id { get; init; }
        public string CustomerId { get; init; }

    }

    public class ChangeDefaultValidator : AbstractValidator<ChangeDefaultRequest>
    {
        public ChangeDefaultValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.CustomerId)
               .NotEmpty();
        }
    }


    public class ChangeDefaultHandler : IRequestHandler<ChangeDefaultRequest, ChangeDefaultResult>
    {
        private readonly IBaseCommandRepository<ShippingAddress> _repository;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeDefaultHandler(
            IBaseCommandRepository<ShippingAddress> repository,
            ICommandContext context,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<ChangeDefaultResult> Handle(ChangeDefaultRequest request, CancellationToken cancellationToken)
        {
            var entityDefault = _context.ShippingAddress.FirstOrDefault(x => x.IsDefault && x.UserId == request.CustomerId);
            if (entityDefault != null)
            {
                entityDefault.IsDefault = false;
            }
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if(entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
            }
            entity.IsDefault = true;
          
            await _unitOfWork.SaveAsync(cancellationToken);

            return new ChangeDefaultResult
            {
                Id = request.Id,
                Message = "Success"
            };
        }
    }
}
