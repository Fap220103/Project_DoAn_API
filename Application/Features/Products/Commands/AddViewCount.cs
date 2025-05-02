using Application.Services.CQS.Queries;
using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Features.Products.Commands
{
    public class AddViewCountResult
    {
        public string Message { get; init; } = null!;
    }
    public class AddViewCountRequest : IRequest<AddViewCountResult>
    {
        public string productId { get; init; }

    }

    public class AddViewCountValidator : AbstractValidator<AddViewCountRequest>
    {
        public AddViewCountValidator()
        {
            RuleFor(x => x.productId)
               .NotEmpty();

        }
    }


    public class AddViewCountHandler : IRequestHandler<AddViewCountRequest, AddViewCountResult>
    {
        private readonly IBaseCommandRepository<Product> _repoProduct;
        private readonly IUnitOfWork _unitOfWork;

        public AddViewCountHandler(
            IBaseCommandRepository<Product> repoProduct,
            IUnitOfWork unitOfWork    
            )
        {
            _repoProduct = repoProduct;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddViewCountResult> Handle(AddViewCountRequest request, CancellationToken cancellationToken = default)
        {
            var product = await _repoProduct.GetByIdAsync(request.productId, cancellationToken);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            product.ViewCount += 1;
            _repoProduct.Update(product);
            await _unitOfWork.SaveAsync(cancellationToken);

            return new AddViewCountResult
            {
                Message = "Success"
            };
        }
    }
}
