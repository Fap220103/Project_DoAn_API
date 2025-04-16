//using Application.Services.Repositories;
//using Domain.Constants;
//using Domain.Entities;
//using FluentValidation;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Features.Colors.Commands
//{
//    public class UpsertColorResult
//    {
//        public string Id { get; init; } = null!;
//        public string Message { get; init; } = null!;
//    }

//    public class UpsertColorRequest : IRequest<UpsertColorResult>
//    {
//        public string? Id { get; init; } 
//        public string ColorName { get; init; } = null!;
//        public string ColorCode { get; init; } = null!;

//    }

//    public class UpsertColorValidator : AbstractValidator<UpsertColorRequest>
//    {
//        public UpsertColorValidator()
//        {
//            RuleFor(x => x.ColorName)
//                .NotEmpty();
//            RuleFor(x => x.ColorCode)
//              .NotEmpty();
//        }
//    }


//    public class UpsertColorHandler : IRequestHandler<UpsertColorRequest, UpsertColorResult>
//    {
//        private readonly IBaseCommandRepository<Color> _repository;
//        private readonly IUnitOfWork _unitOfWork;

//        public UpsertColorHandler(
//            IBaseCommandRepository<Color> repository,
//            IUnitOfWork unitOfWork
//            )
//        {
//            _repository = repository;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<UpsertColorResult> Handle(UpsertColorRequest request, CancellationToken cancellationToken)
//        {

//            Color entity;

//            if (string.IsNullOrWhiteSpace(request.Id))
//            {
//                // Thêm mới
//                entity = new Color
//                {
//                    Id = Guid.NewGuid().ToString(),
//                    ColorName = request.ColorName,
//                    ColorCode = request.ColorCode
//                };

//                await _repository.CreateAsync(entity, cancellationToken);
//            }
//            else
//            {
//                // Cập nhật
//                entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
//                if (entity == null)
//                {
//                    throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.Id}");
//                }

//                entity.Update(request.ColorName, request.ColorCode);
//                _repository.Update(entity);
//            }

//            await _unitOfWork.SaveAsync(cancellationToken);

//            return new UpsertColorResult
//            {
//                Id = entity.Id,
//                Message = "Success"
//            };
//        }
//    }
//}
