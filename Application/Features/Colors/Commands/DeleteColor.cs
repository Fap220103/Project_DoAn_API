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
//    public class DeleteColorResult
//    {
//        public string Id { get; init; } = null!;
//        public string Message { get; init; } = null!;
//    }

//    public class DeleteColorRequest : IRequest<DeleteColorResult>
//    {
//        public string ColorId { get; init; } = null!;
//    }

//    public class DeleteColorValidator : AbstractValidator<DeleteColorRequest>
//    {
//        public DeleteColorValidator()
//        {
//            RuleFor(x => x.ColorId)
//                .NotEmpty();
//        }
//    }


//    public class DeleteColorHandler : IRequestHandler<DeleteColorRequest, DeleteColorResult>
//    {
//        private readonly IBaseCommandRepository<Color> _repository;
//        private readonly IUnitOfWork _unitOfWork;

//        public DeleteColorHandler(
//            IBaseCommandRepository<Color> repository,
//            IUnitOfWork unitOfWork
//            )
//        {
//            _repository = repository;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<DeleteColorResult> Handle(DeleteColorRequest request, CancellationToken cancellationToken = default)
//        {

//            var entity = await _repository.GetByIdAsync(request.ColorId);

//            if (entity == null)
//            {
//                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ColorId}");
//            }

//            _repository.Purge(entity);
//            await _unitOfWork.SaveAsync(cancellationToken);

//            return new DeleteColorResult
//            {
//                Id = entity.Id,
//                Message = "Success"
//            };
//        }
//    }
//}
