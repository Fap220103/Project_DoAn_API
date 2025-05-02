using Application.Services.CQS.Queries;
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

namespace Application.Features.NewsManager.Commands
{
    public class DeleteNewsResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class DeleteNewsRequest : IRequest<DeleteNewsResult>
    {
        public string NewId { get; init; } = null!;
    }

    public class DeleteNewsValidator : AbstractValidator<DeleteNewsRequest>
    {
        public DeleteNewsValidator()
        {

            RuleFor(x => x.NewId)
                .NotEmpty();
        }
    }


    public class DeleteNewsHandler : IRequestHandler<DeleteNewsRequest, DeleteNewsResult>
    {
        private readonly IBaseCommandRepository<News> _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteNewsHandler(
            IBaseCommandRepository<News> repo,
            IUnitOfWork unitOfWork
            )
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteNewsResult> Handle(DeleteNewsRequest request, CancellationToken cancellationToken = default)
        {
            var query = _repo.GetQuery();

            query = query
                .ApplyIsDeletedFilter()
                .Where(x => x.Id == request.NewId);

            var entity = await query.SingleOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.NewId}");
            }

            entity.Delete(null);

            _repo.Update(entity);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new DeleteNewsResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
