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

namespace Application.Features.Reviews.Commands
{
    public class AddReviewResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class AddReviewRequest : IRequest<AddReviewResult>
    {
        public string ProductId { get; set; } = null!;
        public string CustomerId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Rate { get; set; }
    }

    public class AddReviewValidator : AbstractValidator<AddReviewRequest>
    {
        public AddReviewValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
            RuleFor(x => x.CustomerId)
              .NotEmpty();
            RuleFor(x => x.Content)
                .NotEmpty();
            RuleFor(x => x.Rate)
              .NotEmpty();
        }
    }


    public class AddReviewHandler : IRequestHandler<AddReviewRequest, AddReviewResult>
    {
        private readonly IBaseCommandRepository<ReviewProduct> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewHandler(
            IBaseCommandRepository<ReviewProduct> repository,
            IUnitOfWork unitOfWork
            )
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddReviewResult> Handle(AddReviewRequest request, CancellationToken cancellationToken)
        {
            var hasReviewed = await _repository
                                    .AnyAsync(x => x.ProductId == request.ProductId && x.CustomerId == request.CustomerId, cancellationToken);

            if (hasReviewed)
            {
                throw new ApplicationException("Bạn đã đánh giá sản phẩm này rồi!");
  
            }
            var entity = new ReviewProduct(request.ProductId, request.CustomerId, request.Content, request.Rate );
            await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new AddReviewResult
            {
                Id = entity.Id,
                Message = "Success"
            };
        }
    }
}
