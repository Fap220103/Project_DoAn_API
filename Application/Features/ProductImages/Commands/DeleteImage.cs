using Application.Services.Externals;
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

namespace Application.Features.ProductImages.Commands
{
    public class DeleteImageResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteImageRequest : IRequest<DeleteImageResult>
    {
        public string ImageId { get; init; } = null!;
    }

    public class DeleteImageValidator : AbstractValidator<DeleteImageRequest>
    {
        public DeleteImageValidator()
        {
            RuleFor(x => x.ImageId)
                .NotEmpty();
        }
    }


    public class DeleteImageHandler : IRequestHandler<DeleteImageRequest, DeleteImageResult>
    {
        private readonly IBaseCommandRepository<ProductImage> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public DeleteImageHandler(
            IBaseCommandRepository<ProductImage> repository,
            IUnitOfWork unitOfWork,
            IPhotoService photoService
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<DeleteImageResult> Handle(DeleteImageRequest request, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(request.ImageId);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ImageId}");
            }
            var deleteResult = await _photoService.DeletePhotoAsync(entity.Image);
            if (deleteResult == null)
                throw new ApplicationException("Xóa ảnh không thành công");

            _repository.Purge(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteImageResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
