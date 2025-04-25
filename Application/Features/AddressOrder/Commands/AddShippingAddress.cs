using Application.Services.CQS.Commands;
using Application.Services.Externals;
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

namespace Application.Features.AddressOrder.Commands
{
    public class AddShippingAddressResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddShippingAddressRequest : IRequest<AddShippingAddressResult>
    {
        public string UserId { get; init; } = null!;
        public string RecipientName { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;
        public string AddressLine { get; init; } = null!;
        public string Province { get; init; } = null!;
        public string District { get; init; } = null!;
        public string Ward { get; init; } = null!;
        public bool IsDefault { get; init; } = false;
    }

    public class AddShippingAddressHandler : IRequestHandler<AddShippingAddressRequest, AddShippingAddressResult>
    {
        private readonly IBaseCommandRepository<ShippingAddress> _repository;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;

        public AddShippingAddressHandler(
            IBaseCommandRepository<ShippingAddress> repository,
            ICommandContext context,
            IUnitOfWork unitOfWork,
            IIdentityService identityService
            )
        {
            _repository = repository;
            _context = context;
            _unitOfWork = unitOfWork;
            _identityService = identityService;
        }

        public async Task<AddShippingAddressResult> Handle(AddShippingAddressRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _identityService.IsUserExistsAsync(request.UserId, cancellationToken);
            if (!userExists)
            {
                throw new ApplicationException($"Không tìm thấy người dùng với Id: {request.UserId}");
            }

            if (request.IsDefault)
            {
                var oldDefault = _context.ShippingAddress
                    .FirstOrDefault(x => x.UserId == request.UserId && x.IsDefault);

                if (oldDefault != null)
                {
                    oldDefault.IsDefault = false;
                }
            }

            var newAddress = new ShippingAddress(
                request.UserId,
                request.RecipientName,
                request.PhoneNumber,
                request.AddressLine,
                request.Province,
                request.District,
                request.Ward,
                request.IsDefault
            );

            await _repository.CreateAsync(newAddress);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new AddShippingAddressResult
            {
                Id = newAddress.Id,
                Message = "Success"
            };
        }
    }
}
