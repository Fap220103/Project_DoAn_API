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

namespace Application.Features.Settings.Commands
{
    public class UpsertSettingResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpsertSettingRequest : IRequest<UpsertSettingResult>
    {
        public string? Id { get; init; }
        public string Key { get; init; } = null!;
        public string Value { get; init; } = null!;

    }

    public class UpsertSettingValidator : AbstractValidator<UpsertSettingRequest>
    {
        public UpsertSettingValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty();
            RuleFor(x => x.Value)
              .NotEmpty();
        }
    }


    public class UpsertSettingHandler : IRequestHandler<UpsertSettingRequest, UpsertSettingResult>
    {
        private readonly IBaseCommandRepository<Setting> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpsertSettingHandler(
            IBaseCommandRepository<Setting> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpsertSettingResult> Handle(UpsertSettingRequest request, CancellationToken cancellationToken)
        {

            Setting entity;

            if (string.IsNullOrWhiteSpace(request.Id))
            {
                // Thêm mới
                entity = new Setting
                {
                    Id = Guid.NewGuid().ToString(),
                    Key = request.Key,
                    Value = request.Value
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

                entity.Update(request.Key, request.Value);
                _repository.Update(entity);
            }

            await _unitOfWork.SaveAsync(cancellationToken);

            return new UpsertSettingResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
