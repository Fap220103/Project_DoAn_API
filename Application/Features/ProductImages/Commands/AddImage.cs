using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Features.ProductImages.Commands
{
    public class AddImageResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddImageRequest : IRequest<AddImageResult>
    {
        public string? ProductId { get; init; }
        public IFormFile Image { get; init; } = null!;  
    }

    public class AddImageValidator : AbstractValidator<AddImageRequest>
    {
        public AddImageValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
        }
    }


    public class AddImageHandler : IRequestHandler<AddImageRequest, AddImageResult>
    {
        private readonly IBaseCommandRepository<ProductImage> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AddImageHandler(
            IBaseCommandRepository<ProductImage> repository,
            IUnitOfWork unitOfWork,
            IPhotoService photoService
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<AddImageResult> Handle(AddImageRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _photoService.AddPhotoAsync(request.Image);
            var item = new ProductImage
                        (
                            request.ProductId, 
                            result.Url.ToString()
                        );
           

            await _repository.CreateAsync(item, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new AddImageResult
            {
                Id = item.Id,
                Message = "Success"
            };
        }
    }
}
