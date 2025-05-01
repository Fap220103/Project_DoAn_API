using Application.Services.CQS.Commands;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Sizes.Commands
{
    public class CreateSizeResult
    {
        public int Id { get; init; }
        public string Message { get; init; } = null!;
    }

    public class CreateSizeRequest : IRequest<CreateSizeResult>
    {
        public string Name { get; init; } = null!;
    }

    public class CreateSizeValidator : AbstractValidator<CreateSizeRequest>
    {
        public CreateSizeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }


    public class CreateSizeHandler : IRequestHandler<CreateSizeRequest, CreateSizeResult>
    {
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSizeHandler(
            ICommandContext context,
            IUnitOfWork unitOfWork
            )
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateSizeResult> Handle(CreateSizeRequest request, CancellationToken cancellationToken = default)
        {
            var isExist = await _context.Size.AnyAsync(s => s.Name == request.Name, cancellationToken);
            if (isExist)
            {
                return new CreateSizeResult
                {
                    Id = 0,
                    Message = "Size already exists"
                };
            }

            var entity = new Size { Name = request.Name };

            _context.Size.Add(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new CreateSizeResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
