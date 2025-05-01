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

namespace Application.Features.Sizes.Commands
{
    public class DeleteSizeResult
    {
        public int Id { get; init; }
        public string Message { get; init; } = null!;
    }

    public class DeleteSizeRequest : IRequest<DeleteSizeResult>
    {
        public int SizeId { get; init; } 
    }

    public class DeleteSizeValidator : AbstractValidator<DeleteSizeRequest>
    {
        public DeleteSizeValidator()
        {
            RuleFor(x => x.SizeId)
                .NotEmpty();
        }
    }


    public class DeleteSizeHandler : IRequestHandler<DeleteSizeRequest, DeleteSizeResult>
    {
        private readonly ICommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSizeHandler(
            ICommandContext context,
            IUnitOfWork unitOfWork
            )
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteSizeResult> Handle(DeleteSizeRequest request, CancellationToken cancellationToken = default)
        {

            var entity = await _context.Size.FindAsync(request.SizeId);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.SizeId}");
            }

            _context.Size.Remove(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteSizeResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
