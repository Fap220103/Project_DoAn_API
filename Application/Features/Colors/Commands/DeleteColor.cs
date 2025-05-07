using Application.Features.Sizes.Commands;
using Application.Services.CQS.Commands;
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

namespace Application.Features.Colors.Commands
{
    public class DeleteColorResult
    {
        public int Id { get; init; } 
        public string Message { get; init; } = null!;
    }

    public class DeleteColorRequest : IRequest<DeleteColorResult>
    {
        public int ColorId { get; init; } 
    }

    public class DeleteColorValidator : AbstractValidator<DeleteColorRequest>
    {
        public DeleteColorValidator()
        {
            RuleFor(x => x.ColorId)
                .NotEmpty();
        }
    }


    public class DeleteColorHandler : IRequestHandler<DeleteColorRequest, DeleteColorResult>
    {
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteColorHandler(
            ICommandContext context,
            IUnitOfWork unitOfWork
            )
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteColorResult> Handle(DeleteColorRequest request, CancellationToken cancellationToken = default)
        {

            var entity = await _context.Color.FindAsync(request.ColorId);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ColorId}");
            }

            _context.Color.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteColorResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
