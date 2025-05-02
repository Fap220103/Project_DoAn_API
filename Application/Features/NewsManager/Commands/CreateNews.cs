using Application.Services.CQS.Commands;
using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Features.NewsManager.Commands
{
    public class AddNewsResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddNewsRequest : IRequest<AddNewsResult>
    {
     
        public string Detail { get; init; } = null!;
        public string Title { get; init; } = null!;
        public IFormFile? Image { get; init; }
        public string? Link { get; init; }
        public string Description { get; init; } = null!;
        public string Type { get; init; } = null!;

    }

    public class AddNewsHandler : IRequestHandler<AddNewsRequest, AddNewsResult>
    {
        private readonly IBaseCommandRepository<News> _repository;
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AddNewsHandler(
            IBaseCommandRepository<News> repository,
            ICommandContext context,
            IUnitOfWork unitOfWork,
            IPhotoService photoService
            )
        {
            _repository = repository;
            _context = context;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        public async Task<AddNewsResult> Handle(AddNewsRequest request, CancellationToken cancellationToken)
        {
            var entity = new News
           (
                null,
                request.Title,
                request.Description,
                null,
                request.Detail,
                request.Link,
                request.Type
            );
            if (request.Image != null)
            {
                var result = await _photoService.AddPhotoAsync(request.Image);
                entity.Image = result.Url.ToString();
            }
            await _repository.CreateAsync(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new AddNewsResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
