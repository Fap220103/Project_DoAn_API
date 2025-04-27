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
    public class UpdateShippingAddressResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateShippingAddressRequest : IRequest<UpdateShippingAddressResult>
    {
        public string Id { get; init; } = null!;
        public string UserId { get; init; } = null!;
        public string RecipientName { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;
        public string AddressLine { get; init; } = null!;
        public string Province { get; init; } = null!;
        public string ProvinceCode { get; init; } = null!;
        public string District { get; init; } = null!;
        public string DistrictCode { get; init; } = null!;
        public string Ward { get; init; } = null!;
        public string WardCode { get; init; } = null!;
        public bool IsDefault { get; init; } = false;
    }

    public class UpdateShippingAddressHandler : IRequestHandler<UpdateShippingAddressRequest, UpdateShippingAddressResult>
    {
        private readonly IBaseCommandRepository<ShippingAddress> _repository;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;

        public UpdateShippingAddressHandler(
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

        public async Task<UpdateShippingAddressResult> Handle(UpdateShippingAddressRequest request, CancellationToken cancellationToken)
        {
            var userExists = await _identityService.IsUserExistsAsync(request.UserId, cancellationToken);
            if (!userExists)
            {
                throw new ApplicationException($"Không tìm thấy người dùng với Id: {request.UserId}");
            }
            var addressExits = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if(addressExits == null)
            {
                throw new ApplicationException($"Không tìm thấy địa chỉ với Id: {request.Id}");
            }

            if (request.IsDefault)
            {
                var oldDefault = _context.ShippingAddress
                    .FirstOrDefault(x => x.UserId == request.UserId && x.IsDefault);

                if (oldDefault != null)
                {
                    oldDefault.IsDefault = false;
                    _context.ShippingAddress.Update(oldDefault);
                    await _context.SaveChangesAsync();
                }
            }

            addressExits.Update(
                request.RecipientName,
                request.PhoneNumber,
                request.AddressLine,
                request.Province,
                request.District,
                request.Ward,
                request.ProvinceCode,
                request.DistrictCode,
                request.WardCode,
                request.IsDefault
            );

            _repository.Update(addressExits);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpdateShippingAddressResult
            {
                Id = addressExits.Id,
                Message = "Success"
            };
        }
    }
}
