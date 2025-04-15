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

namespace Application.Features.Colors.Commands
{
    public class UpsertSizeResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpsertSizeRequest : IRequest<UpsertSizeResult>
    {
        public string? Id { get; init; }
        public string SizeName { get; init; } = null!;

    }

    public class UpsertSizeValidator : AbstractValidator<UpsertSizeRequest>
    {
        public UpsertSizeValidator()
        {
            RuleFor(x => x.SizeName)
              .NotEmpty();
        }
    }


    public class UpsertSizeHandler : IRequestHandler<UpsertSizeRequest, UpsertSizeResult>
    {
        private readonly IBaseCommandRepository<Size> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpsertSizeHandler(
            IBaseCommandRepository<Size> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpsertSizeResult> Handle(UpsertSizeRequest request, CancellationToken cancellationToken)
        {

            Size entity;

            if (string.IsNullOrWhiteSpace(request.Id))
            {
                // Thêm mới
                entity = new Size
                {
                    SizeName = request.SizeName
                };

                await _repository.CreateAsync(entity, cancellationToken);
            }
            else
            {
                // Cập nhật
                entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
                if (entity == null)
                {
                    throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
                }

                entity.Update(request.SizeName);
                _repository.Update(entity);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpsertSizeResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
