using Application.Services.Externals;
using Application.Services.Repositories;
using Domain.Constants;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Application.Features.ShoppingCarts.Commands
{
    public class UpdateCartResult
    {
        public string Id { get; init; } = null!;
        public string Message { get; init; } = null!;
    }

    public class UpdateCartRequest : IRequest<UpdateCartResult>
    {
        public string ProductId { get; init; } = null!;
        public int Quantity { get; init; }

    }

    public class UpdateCartValidator : AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty();
            RuleFor(x => x.Quantity)
                .NotEmpty();
        }
    }


    public class UpdateCartHandler : IRequestHandler<UpdateCartRequest, UpdateCartResult>
    {
        private readonly ICartSessionService _cartSessionService;

        public UpdateCartHandler(
             ICartSessionService cartSessionService
            )
        {
            _cartSessionService = cartSessionService;
        }

        public async Task<UpdateCartResult> Handle(UpdateCartRequest request, CancellationToken cancellationToken)
        {

            ShoppingCart cart = _cartSessionService.GetCart();
            if (cart != null)
            {
                cart.UpdateQuantity(request.ProductId, request.Quantity);
                _cartSessionService.SetCart(cart);
            }
            else
            {
                throw new ApplicationException($"{ExceptionConsts.EntitiyNotFound} {request.ProductId}");
            }

            return new UpdateCartResult
            {
                Id = request.ProductId,
                Message = "Success"
            };
        }
    }
}
